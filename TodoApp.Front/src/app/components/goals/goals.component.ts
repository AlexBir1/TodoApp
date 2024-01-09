import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GoalService } from 'src/app/services/implementations/goal.service';
import { CategoryModel } from 'src/app/shared/models/category.model';
import { CollectionModel } from 'src/app/shared/models/collection.model';
import { GoalModel } from 'src/app/shared/models/goal.model';

@Component({
  selector: 'app-goals',
  templateUrl: './goals.component.html',
  styleUrls: ['./goals.component.css']
})
export class GoalsComponent{
  isCreationMode: boolean = false;
  goalForm!: FormGroup;

  selectedCategoryId: string = '';

  @Input() goals: GoalModel[] = [];
  @Input() collection!: CollectionModel;
  @Input() collections: CollectionModel[] = [];
  @Input() categories: CategoryModel[] = [];

  @Output() selectedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() deletedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() updatedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() changedSelectedCategoryEvent: EventEmitter<string> = new EventEmitter<string>();

  constructor(private goalService: GoalService){}

  onSelectedCategoryChange(){
    this.changedSelectedCategoryEvent.emit(this.selectedCategoryId);
  }

  onChangeGoalCompletion(goal: GoalModel, sender: any, event: MouseEvent){
    event.stopPropagation();
    goal.isCompleted = sender.target.checked;
    this.goalService.update(goal.id, goal).subscribe({
      next: (result) => {
        if(result.isSuccess){
          let index = this.goals.findIndex(x=>x.id === result.data.id);
          this.goals[index] = result.data;
          this.updatedGoalEvent.emit(result.data);
        }
      },
      error: (error) => {

      }
    });
  }

  createGoal(){
    var goal: GoalModel = this.goalForm.value;
    this.goalService.create(goal).subscribe({
      next: (result) =>{
        if(result.isSuccess){
          this.goals.push(result.data);
          this.isCreationMode = false;
        }
      },
      error: (error) => {

      }
    });
  }

  deleteGoal(id: string, event: MouseEvent){
    event.stopPropagation();
    this.goalService.delete(id).subscribe({
      next: (result) =>{
        if(result.isSuccess){
          let index = this.goals.findIndex(x=>x.id === id);
          this.goals.splice(index, 1);
          this.deletedGoalEvent.emit(result.data);
        }
      },
      error: (error) => {

      }
    });
  }

  selectGoal(goal: GoalModel){
    this.selectedGoalEvent.emit(goal);
  }

  changeIsCreationMode(){
    this.isCreationMode = !this.isCreationMode;
    if(this.isCreationMode)
      this.initializeSignUpForm();
  }

  initializeSignUpForm(){
    this.goalForm = new FormGroup({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      startDate: new FormControl('', Validators.required),
      collectionId: new FormControl('', Validators.required),
    });
  }
}
