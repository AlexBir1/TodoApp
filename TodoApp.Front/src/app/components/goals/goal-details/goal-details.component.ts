import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CategoryService } from 'src/app/services/implementations/category.service';
import { GoalService } from 'src/app/services/implementations/goal.service';
import { CategoryModel } from 'src/app/shared/models/category.model';
import { GoalCategory } from 'src/app/shared/models/goal-category.model';
import { GoalModel } from 'src/app/shared/models/goal.model';

@Component({
  selector: 'app-goal-details',
  templateUrl: './goal-details.component.html',
  styleUrls: ['./goal-details.component.css']
})
export class GoalDetailsComponent implements OnInit, OnChanges{
  @Input() goal!: GoalModel | null;
  @Input() categories: CategoryModel[] = [];
  @Output() closedGoalDetailsEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() deletedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() updatedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();

  @Output() addedToCategoryEvent: EventEmitter<GoalCategory> = new EventEmitter<GoalCategory>();
  @Output() removedFromCategoryEvent: EventEmitter<GoalCategory> = new EventEmitter<GoalCategory>();

  goalForm!: FormGroup;
  isAddingCategoryMode: boolean = false;


  constructor(private goalService: GoalService){
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.initializeGoalForm();
  }
  ngOnInit(): void {
    this.initializeGoalForm();
  }

  updateGoal(){
    let newGoal = this.goalForm.value;
    this.goalService.update(newGoal.id, newGoal).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.goal = result.data;
          this.updatedGoalEvent.emit(result.data);
        }
      },
      error: (error) => {

      }
    });
  }

  changeIsAddingCategoryMode(){
    this.isAddingCategoryMode = !this.isAddingCategoryMode;
  }

  selectCategoryToAdd(category: CategoryModel){
    this.goalService.addToCategory(this.goal!.id, category.id).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.goal?.goalCategories.push(result.data);
          this.addedToCategoryEvent.emit(result.data);
        }
      },
      error: (error) => {

      }
    });
  }

  selectCategoryToRemove(goalCategory: GoalCategory){
    this.goalService.removeFromCategory(this.goal!.id, goalCategory.categoryId).subscribe({
      next: (result) => {
        if(result.isSuccess){
          let index = this.goal!.goalCategories.findIndex(x=>x.categoryId == goalCategory.categoryId);
          this.goal!.goalCategories.splice(index,1);
          this.removedFromCategoryEvent.emit(result.data);
        }
      },
      error: (error) => {

      }
    });
  }

  closeDetails(){
    this.goal = null;
    this.closedGoalDetailsEvent.emit();
  }

  initializeGoalForm(){
    this.goalForm = new FormGroup({
      id: new FormControl('', Validators.required),
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      startDate: new FormControl('', Validators.required),
      collectionId: new FormControl('', Validators.required),
      isCompleted: new FormControl(false, Validators.required),
    });

    this.goalForm.controls['id'].setValue(this.goal?.id);
    this.goalForm.controls['title'].setValue(this.goal?.title);
    this.goalForm.controls['description'].setValue(this.goal?.description);
    this.goalForm.controls['startDate'].setValue(this.goal?.startDate);
    this.goalForm.controls['collectionId'].setValue(this.goal?.collectionId);
    this.goalForm.controls['isCompleted'].setValue(this.goal?.isCompleted);
  }

  deleteGoal(id: string){
    this.goalService.delete(id).subscribe({
      next: (result) =>{
        if(result.isSuccess){
          this.goal = null;
          this.deletedGoalEvent.emit(result.data);
        }
      },
      error: (error) => {

      }
    });
  }
}
