import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs';
import { AccountService } from 'src/app/services/implementations/account.service';
import { AuthorizedAccountService } from 'src/app/services/implementations/authorized-account.service';
import { CategoryService } from 'src/app/services/implementations/category.service';
import { CollectionService } from 'src/app/services/implementations/collection.service';
import { GoalService } from 'src/app/services/implementations/goal.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { CategoryModel } from 'src/app/shared/models/category.model';
import { CollectionModel } from 'src/app/shared/models/collection.model';
import { GoalCategory } from 'src/app/shared/models/goal-category.model';
import { GoalModel } from 'src/app/shared/models/goal.model';

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

  constructor(private localStorage: LocalStorageService, private accountService: AccountService, public aaService: AuthorizedAccountService,
    private goalService: GoalService, private collectionService: CollectionService, private categoryService: CategoryService, private router: Router){}
  ngOnInit(): void {
    this.refreshAuthToken();
    this.refresh();
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
    this.goalService.getAllFiltered('', query).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.goals = result.data;
        }
        else{
          this.goals = [];
        }
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  onChangedSelectedCategory(categoryId: string){
    if(categoryId){
      let category = this.categories.find(x=>x.id === categoryId);
      this.goalService.getAllFiltered(this.selectedCollection!.id, category!.colorTitle).subscribe({
        next: (result) => {
          if(result.isSuccess){
            this.goals = result.data;
          }
          else{
            this.goals = [];
          }
        },
        error: (error) => {
          console.log(error);
        },
      });
    }
    else{
      this.goalService.getAllFiltered(this.selectedCollection!.id).subscribe({
        next: (result) => {
          if(result.isSuccess){
            this.goals = result.data;
          }
          else{
            this.goals = [];
          }
        },
        error: (error) => {
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
    this.selectedGoal!.goalCategories.push(goalCategory);
  }

  onGoalRemovedFromCategory(goalCategory: GoalCategory){
    let index = this.selectedGoal!.goalCategories.findIndex(x=>x.categoryId === goalCategory.categoryId && x.goalId === goalCategory.goalId);
    this.selectedGoal!.goalCategories.splice(index,1);
  }

  getAllGoalsInCollection(collection: CollectionModel){
    this.selectedCollection = collection;
    this.goalService.getAllFiltered(collection.id).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.goals = result.data;
        }
        else{
          this.goals = [];
        }
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  getCollections(){
    this.collectionService.getAllFiltered(this.account.accountId).pipe(map(x=>{
      this.selectedCollection = x.data.find(x=>x.title === 'Unsorted') as CollectionModel;
      this.getAllGoalsInCollection(this.selectedCollection);
      return x;
    })).subscribe({
      next: (result) =>{
        if(result.isSuccess)
          this.collections = result.data;
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  getCategories(){
    this.categoryService.getAllFiltered(this.account.accountId).subscribe({
      next: (result) =>{
        if(result.isSuccess)
          this.categories = result.data;
      },
      error: (error) => {
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
