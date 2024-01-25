import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs';
import { AccountService } from 'src/app/services/implementations/account.service';
import { AuthorizedAccountService } from 'src/app/services/implementations/authorized-account.service';
import { CategoryService } from 'src/app/services/implementations/category.service';
import { CollectionService } from 'src/app/services/implementations/collection.service';
import { GoalService } from 'src/app/services/implementations/goal.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { SignalRService } from 'src/app/services/signalr.service';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { CategoryModel } from 'src/app/shared/models/category.model';
import { CollectionModel } from 'src/app/shared/models/collection.model';
import { GoalCategory } from 'src/app/shared/models/goal-category.model';
import { GoalModel } from 'src/app/shared/models/goal.model';
import { PageParams } from 'src/app/shared/page-params';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit{
  isNavbarShowing: boolean = false;
  account!: AuthorizationModel;

  goals: GoalModel[] = [];
  collections: CollectionModel[] = [];
  categories: CategoryModel[] = [];

  selectedCollection!: CollectionModel;
  selectedGoal!: GoalModel | null;
  message!: string | null;

  pageParams!: PageParams;
  responseErrors: string[] | null = null;
  selectedCategoryId: string | null = null;

  isLoadingState: boolean = false;
  isMenuOpened: boolean = false;
  isLogoutModalActive: boolean = false;
  
  constructor(private localStorage: LocalStorageService, private accountService: AccountService, public aaService: AuthorizedAccountService,
    private goalService: GoalService, private collectionService: CollectionService, private categoryService: CategoryService, private router: Router, 
    private signalRService: SignalRService){}
    
  ngOnInit(): void {
    this.pageParams = new PageParams(1,1,10);
    this.signalRService.startConnection();
    this.signalRService.listen();
    this.signalRService.messageSource.subscribe(x=>this.message = x);
    this.refreshAuthToken();
    this.refresh();
  }

  changeIsLogoutModalActive(){
    this.isLogoutModalActive = !this.isLogoutModalActive;
  }

  onChangeMenuOpened(){
    this.isMenuOpened = !this.isMenuOpened;
  }

  onEnableLoadingState(){
    this.isLoadingState = true;
  }

  onDisableLoadingState(){
    this.isLoadingState = false;
  }

  onClosedNotification(){
    this.message = null;
  }

  onClosedError(){
    this.responseErrors = null;
  }

  setResponseErrors(errors: string[]){
    this.responseErrors = errors;
  }

  onGoalsPageChanged(pageParams: PageParams){
    this.isLoadingState =  true;
    this.pageParams = pageParams;
    if(this.selectedCategoryId){
      let category = this.categories.find(x=>x.id === this.selectedCategoryId);
      this.goalService.getAllFiltered(this.selectedCollection.id, category?.colorTitle, pageParams.itemsPerPage, pageParams.selectedPage).subscribe({
        next: (result) => {
          this.isLoadingState = false;
          if(result.isSuccess){
            this.goals = result.data;
            this.pageParams = new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage);
          }
          else{
            this.responseErrors = result.messages;
            this.goals = [];
            this.pageParams = new PageParams(1,1,1);
          }
        },
        error: (error) => {
          this.isLoadingState = false;
          console.log(error);
        },
      });
    }
    else
    this.isLoadingState =  true;
      this.goalService.getAllFiltered(this.selectedCollection.id, '', pageParams.itemsPerPage, pageParams.selectedPage).subscribe({
        next: (result) => {
          this.isLoadingState = false;
          if(result.isSuccess){
            this.goals = result.data;
            this.pageParams = new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage);
          }
          else{
            this.responseErrors = result.messages;
            this.goals = [];
            this.pageParams = new PageParams(1,1,1);
          }
        },
        error: (error) => {
          this.isLoadingState = false;
          console.log(error);
        },
      });
  }

  setSelectedGoal(goal: GoalModel){
    this.selectedGoal = goal;
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
    this.isLoadingState =  true;
      if(query.length > 0){
      this.goalService.getAllFiltered('', query, this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          this.isLoadingState = false;
          if(result.isSuccess){
            this.goals = result.data;
            this.pageParams = new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage);
          }
          else{
            this.responseErrors = result.messages;
            this.goals = [];
            this.pageParams = new PageParams(1,1,1);
          }
        },
        error: (error) => {
          this.isLoadingState = false;
          console.log(error);
        },
      });
    }
    else{
      this.goalService.getAllFiltered(this.selectedCollection.id, '', this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          this.isLoadingState = false;
          if(result.isSuccess){
            this.goals = result.data;
            this.pageParams = new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage);
          }
          else{
            this.responseErrors = result.messages;
            this.goals = [];
            this.pageParams = new PageParams(1,1,1);
          }
        },
        error: (error) => {
          this.isLoadingState = false;
          console.log(error);
        },
      });
    }
  }

  onChangedSelectedCategory(categoryId: string){
    this.isLoadingState =  true;
    this.selectedCategoryId = categoryId;
    if(categoryId){
      let category = this.categories.find(x=>x.id === categoryId);
      this.goalService.getAllFiltered(this.selectedCollection!.id, category!.colorTitle, this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          this.isLoadingState = false;
          if(result.isSuccess){
            this.goals = result.data;
            this.pageParams = new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage);
            this.selectedCategoryId = categoryId;
          }
          else{
            this.responseErrors = result.messages;
            this.goals = [];
            this.pageParams = new PageParams(1,1,1);
            this.selectedCategoryId = null;
          }
        },
        error: (error) => {
          this.isLoadingState = false;
          console.log(error);
        },
      });
    }
    else{
      this.goalService.getAllFiltered(this.selectedCollection!.id, '', this.pageParams.itemsPerPage, this.pageParams.selectedPage).subscribe({
        next: (result) => {
          this.isLoadingState = false;
          if(result.isSuccess){
            this.goals = result.data;
            this.pageParams = new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage);
            this.selectedCategoryId = null;
          }
          else{
            this.responseErrors = result.messages;
            this.goals = [];
            this.pageParams = new PageParams(1,1,1);
            this.selectedCategoryId = null;
          }
        },
        error: (error) => {
          this.isLoadingState = false;
          console.log(error);
        },
      });
    }
  }

  removeSelectedGoal(){
    this.selectedGoal = null;
  }

  setDefaultCollectionAfterDelete(collection: CollectionModel){
    if(this.selectedCollection!.id === collection.id){
      this.selectedCollection = this.collections.find(x=>x.title === "Unsorted") as CollectionModel;
      this.getAllGoalsInCollection(this.selectedCollection);
    }
  }

  refresh(){
    this.account = this.localStorage.getAccountFromStorage();
    this.refreshAuthToken();
    if(this.account){
      this.getCollections();
      this.getCategories();
      this.getAllGoalsInCollection(this.selectedCollection!);
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
    this.isLoadingState =  true;
    this.goalService.getAllFiltered(collection.id, '', this.pageParams.itemsPerPage).subscribe({
      next: (result) => {
        this.isLoadingState = false;
        if(result.isSuccess){
          this.goals = result.data;
          this.pageParams = new PageParams(result.itemsCount, result.selectedPage, result.itemsPerPage);
        }
        else{
          this.responseErrors = result.messages;
          this.goals = [];
          this.pageParams = new PageParams(1,1,1);
        }
      },
      error: (error) => {
        this.isLoadingState = false;
        console.log(error);
      },
    });
  }

  getCollections(){
    this.isLoadingState =  true;
    this.collectionService.getAllFiltered(this.account.accountId).pipe(map(x=>{
      this.selectedCollection = x.data.find(x=>x.title === 'Unsorted') as CollectionModel;
      return x;
    })).subscribe({
      next: (result) =>{
        this.isLoadingState = false;
        if(result.isSuccess)
          this.collections = result.data;
        else 
          this.responseErrors = result.messages;
      },
      error: (error) => {
        this.isLoadingState = false;
        console.log(error);
      }
    })
    .add(() => this.getAllGoalsInCollection(this.selectedCollection));
  }

  getCategories(){
    this.isLoadingState =  true;
    this.categoryService.getAllFiltered(this.account.accountId).subscribe({
      next: (result) => {
        this.isLoadingState = false;
        if(result.isSuccess)
          this.categories = result.data;
      },
      error: (error) => {
        this.isLoadingState = false;
        console.log(error);
      }
    })
  }

  logOut(){
    this.localStorage.removeAccountFromStorage();
    this.aaService.removeAccount();
    this.router.navigateByUrl('/Auth');
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

  changeIsNavbarShowing(){
    this.isNavbarShowing = !this.isNavbarShowing;
  }
}
