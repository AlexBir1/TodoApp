import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { concatMap, map, tap } from 'rxjs';
import { AccountService } from 'src/app/services/implementations/account.service';
import { AuthorizedAccountService } from 'src/app/services/implementations/authorized-account.service';
import { CategoryService } from 'src/app/services/implementations/category.service';
import { CollectionService } from 'src/app/services/implementations/collection.service';
import { GoalNotificationsService } from 'src/app/services/implementations/goal-notifications.service';
import { GoalService } from 'src/app/services/implementations/goal.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { CategoryModel } from 'src/app/shared/models/category.model';
import { CollectionModel } from 'src/app/shared/models/collection.model';
import { GoalCategory } from 'src/app/shared/models/goal-category.model';
import { GoalModel } from 'src/app/shared/models/goal.model';
import { UserNotificationModel } from 'src/app/shared/models/user-notification.model';
import { PageParams } from 'src/app/shared/page-params';

const fadeInOutLeft = trigger('fadeInOutLeft', [
  transition(':enter', [
      style({ opacity: 1, transform: 'translate(-100%)' }),
      animate('350ms ease-out', style({ opacity: 1, transform: 'translate(0%)' })),
  ]),
  transition(':leave', [
    animate('350ms ease-in', style({ opacity: 0, transform: 'translate(-100%)' })),
])
]);

const fadeInOutRight = trigger('fadeInOutRight', [
  transition(':enter', [
    style({ opacity: 1, transform: 'translate(100%)' }),
      animate('350ms ease-out', style({ opacity: 1, transform: 'translate(0%)' })),
  ]),
  transition(':leave', [
    animate('350ms ease-in', style({ opacity: 0, transform: 'translate(100%)' })),
])
]);

const centralfadeInOutRight = trigger('fadeInOutRight', [
  transition(':enter', [
    style({ opacity: 1, transform: 'translate(100%)' }),
      animate('350ms ease-out', style({ opacity: 1, transform: 'translate(0%)' })),
  ]),
  transition(':leave', [
    animate('350ms ease-in', style({ opacity: 0, transform: 'translate(100%)' })),
])
]);


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  animations: [fadeInOutLeft,fadeInOutRight]
})
export class DashboardComponent implements OnInit{
  isNavbarShowing: boolean = false;
  account!: AuthorizationModel;

  goals: GoalModel[] = [];
  collections: CollectionModel[] = [];
  categories: CategoryModel[] = [];

  selectedCollection!: CollectionModel;
  selectedGoal: GoalModel | null = null;
  message!: string | null;
  goalNotification: UserNotificationModel | null = null;

  pageParams: PageParams = new PageParams(1,1,10);;
  responseErrors: string[] | null = null;
  selectedCategoryId: string | null = null;

  isLoadingState: boolean = false;
  isMenuOpened: boolean = false;
  isLogoutModalActive: boolean = false;
  
  constructor(private localStorage: LocalStorageService, private accountService: AccountService, public aaService: AuthorizedAccountService,
    private goalService: GoalService, private collectionService: CollectionService, private categoryService: CategoryService, private router: Router, 
    private signalRService: SignalRService, private goalNotificationsService: GoalNotificationsService){}
  
  ngOnInit(): void {
    this.setupSignalR();
    this.refreshAuthToken();
    this.refreshPageContent();
  }

  setupSignalR(){
    this.signalRService.startConnection();
    this.signalRService.startListen();
    this.signalRService.messageSource.subscribe(x=>this.message = x);
    this.signalRService.goalNotificationSource.subscribe(x=>this.goalNotification = x);
  }

  setSelectedGoalFromNotification(goalId: string){
    let goal = this.goals.find(x=>x.id == goalId)!;
    this.setSelectedGoal(goal);
  }

  changeIsLogoutModalActive(){
    this.isLogoutModalActive = !this.isLogoutModalActive;
  }

  onClosedNotification(){
    this.message = null;
  }

  onClosedGoalNotification(){
    this.goalNotification = null;
  }

  disableLoadingState(){
    this.isLoadingState = false;
  }

  onChangeMenuOpened(){
    this.isMenuOpened = !this.isMenuOpened;
  }

  enableLoadingState(){
    this.isLoadingState = true;
  }

  onClosedError(){
    this.responseErrors = null;
  }

  setSelectedGoal(goal: GoalModel){
    this.selectedGoal = goal;
  }

  setResponseErrors(errors: string[]){
    this.responseErrors = errors;
  }

  onCreatedCollection(collection: CollectionModel){
    this.collections.push(collection);
  }

  onDeletedGoal(){
    this.removeSelectedGoal(true);
  }

  changeIsNavbarShowing(){
    this.isNavbarShowing = !this.isNavbarShowing;
  }

  logOut(){
    this.localStorage.removeAccountFromStorage();
    this.aaService.removeAccount();
    this.router.navigateByUrl('/Auth');
  }

  setupGoalsPage(goals: GoalModel[], params: PageParams, errors: string[] = []){
    this.goals = goals;
    this.pageParams = params;
    if(errors.length > 0)
      this.responseErrors = errors;
  }

  updateGoalsCategoriesAfterDelete(category: CategoryModel){
    this.goals.forEach(x=>{
      let index = x.goalCategories.findIndex(x=>x.categoryId === category.id);
      if(index != -1)
        x.goalCategories.splice(index, 1);
    });
    let indexInSelected = this.selectedGoal!.goalCategories.findIndex(x=>x.categoryId === category.id);
    if(indexInSelected != -1)
      this.selectedGoal!.goalCategories.splice(indexInSelected,1);
  }

  onGoalsPageChanged(pageParams: PageParams){
    this.enableLoadingState();
    this.pageParams = pageParams;
    if(this.selectedCategoryId){
      let category = this.categories.find(x=>x.id === this.selectedCategoryId);
      this.goalService.getAllFiltered(this.selectedCollection.id, category?.colorTitle, pageParams.itemsPerPage, pageParams.selectedPage).subscribe({
        next: (result) => {
          if(result.isSuccess)
            this.setupGoalsPage(result.data, new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage));
          else
            this.setupGoalsPage([], new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage), result.messages);
        },
        error: (error) => {
          console.log(error);
        },
      }).add(() => this.disableLoadingState());
    }
    else{
      this.goalService.getAllFiltered(this.selectedCollection.id, '', pageParams.itemsPerPage, pageParams.selectedPage).subscribe({
        next: (result) => {
          if(result.isSuccess)
            this.setupGoalsPage(result.data, new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage));
          else
            this.setupGoalsPage([], new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage), result.messages);
        },
        error: (error) => {
          console.log(error);
        },
      }).add(() => this.disableLoadingState());
    }
  }

  onUpdatedGoal(goal: GoalModel){
    let index = this.goals.findIndex(x=>x.id === goal.id);
    this.goals[index] = goal;
    if(this.selectedGoal?.id === goal.id){
      this.selectedGoal = goal;
    }
  }

  onSearchGoalsByQuery(query: string){
    this.selectedGoal = null;
    this.enableLoadingState();
      if(query.length > 0){
      this.goalService.getAllFiltered('', query, this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          if(result.isSuccess)
            this.setupGoalsPage(result.data, new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage));
          else
            this.setupGoalsPage([], new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage), result.messages);
        },
        error: (error) => {
          console.log(error);
        },
      }).add(() => this.disableLoadingState());
    }
    else{
      this.goalService.getAllFiltered(this.selectedCollection.id, '', this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          if(result.isSuccess)
            this.setupGoalsPage(result.data, new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage));
          else
            this.setupGoalsPage([], new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage), result.messages);
        },
        error: (error) => {
          console.log(error);
        },
      }).add(() => this.disableLoadingState());
    }
  }

  onChangedSelectedCategory(categoryId: string){
    this.selectedCategoryId = categoryId;
    this.enableLoadingState();

    if(!this.selectedCollection){
      this.disableLoadingState();
      return;
    }
      
    if(categoryId){
      let category = this.categories.find(x=>x.id === categoryId);
      this.goalService.getAllFiltered(this.selectedCollection!.id, category!.colorTitle, this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          if(result.isSuccess)
            this.setupGoalsPage(result.data, new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage));
          else
            this.setupGoalsPage([], new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage), result.messages);
        },
        error: (error) => {
          console.log(error);
        },
      }).add(() => this.disableLoadingState());
    }
    else{
      this.goalService.getAllFiltered(this.selectedCollection!.id, '', this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          this.selectedCategoryId = null;
          if(result.isSuccess){
            this.setupGoalsPage(result.data, new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage));
          }
          else{
            this.setupGoalsPage([], new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage), result.messages);
          }
        },
        error: (error) => {
          console.log(error);
        },
      }).add(() => this.disableLoadingState());
    }
  }

  removeSelectedGoal(removeGoalInList: boolean = false){
    if(removeGoalInList){
      var index = this.goals.findIndex(x=>x.id === this.selectedGoal?.id);
      if(index != -1)
        this.goals.splice(index,1);
    }
    this.selectedGoal = null;
  }

  setDefaultCollectionAfterDelete(collection: CollectionModel){
    if(this.selectedCollection!.id === collection.id){
      let unsorted = this.collections.find(x=>x.title === "Unsorted") as CollectionModel;
      this.selectedCollection = unsorted;
      this.getAllGoalsInCollection(this.selectedCollection);
    }
  }

  refreshPageContent(){
    this.account = this.localStorage.getAccountFromStorage();
    this.refreshAuthToken();
    if(this.account){
      this.getCategories();
      this.getCollectionsAndGoals();
    }
  }

  onGoalAddedToCategory(goalCategory: GoalCategory){
    let index = this.goals.findIndex(x=>x.id === goalCategory.goalId);
    let category = this.categories.find(x=>x.id === goalCategory.categoryId);
    goalCategory.category = category as CategoryModel;
    this.goals[index].goalCategories.push(goalCategory);
  }

  onGoalRemovedFromCategory(goalCategory: GoalCategory){
    let index = this.goals.find(x=>x.id === goalCategory.goalId)!.goalCategories.findIndex(x=>x.categoryId === goalCategory.categoryId);
    this.goals.find(x=>x.id === goalCategory.goalId)!.goalCategories.splice(index,1);
  }

  getAllGoalsInCollection(collection: CollectionModel){
    this.selectedCollection = collection;
    this.enableLoadingState();
    this.goalService.getAllFiltered(collection.id, '', this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.setupGoalsPage(result.data, new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage));
        }
        else{
          this.setupGoalsPage([], new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage), result.messages);
        }
      },
      error: (error) => {
        console.log(error);
      },
    }).add(() => { this.disableLoadingState(); this.goalNotificationsService.create(this.goals).subscribe();});
  }

  getCollectionsAndGoals(){
    this.enableLoadingState();
    this.collectionService.getAllFiltered(this.account.accountId).pipe(map(x=>{
      this.selectedCollection = x.data.find(x=>x.title === 'Unsorted') as CollectionModel;
      return x;
    }))
    .subscribe({
      next: (result) =>{
        if(result.isSuccess) {
          this.collections = result.data;
          this.getAllGoalsInCollection(this.selectedCollection);
        }
        else{
          this.responseErrors = result.messages;
        }
      },
      error: (error) => {
        console.log(error);
      },
    }).add(() => this.disableLoadingState());
  }

  getCategories(){
    this.isLoadingState = true;
    this.categoryService.getAllFiltered(this.account.accountId).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.categories = result.data;
        }
      },
      error: (error) => {
        console.log(error);
      }
    }).add(() => this.disableLoadingState());
  }

  refreshAuthToken(){
    this.account = this.localStorage.getAccountFromStorage();
    if(this.account){
      if(this.account.keepAuthorized){
        if(!this.validateToken(this.account)){
            this.accountService.refreshAuthToken(this.account).subscribe({
                next: (r) => {
                    if(r.isSuccess){
                        this.localStorage.addAccountToStorage(r.data);
                        this.aaService.addAccount(r.data);
                    }
                },
                error: (e) =>{
                  console.log(e);
                }
            });
        }
      }
      else{
          if(!this.validateToken(this.account)){
              this.localStorage.removeAccountFromStorage();
              this.aaService.removeAccount();
          }
      }
    }
  }

  validateToken(model: AuthorizationModel): boolean{
    var date = Date.now();
    var expirationDate = new Date(model.tokenExpirationDate).getTime();
    if(date > expirationDate) 
        return false;
    return true
  }
}
