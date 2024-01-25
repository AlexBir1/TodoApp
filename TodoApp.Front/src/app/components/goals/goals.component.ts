import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GoalService } from 'src/app/services/implementations/goal.service';
import { APIResponse } from 'src/app/shared/api-response/api-response';
import { CategoryModel } from 'src/app/shared/models/category.model';
import { CollectionModel } from 'src/app/shared/models/collection.model';
import { GoalModel } from 'src/app/shared/models/goal.model';
import { PageParams } from 'src/app/shared/page-params';

@Component({
  selector: 'app-goals',
  templateUrl: './goals.component.html',
  styleUrls: ['./goals.component.css']
})
export class GoalsComponent implements OnChanges{
  isCreationMode: boolean = false;
  goalForm!: FormGroup;
  selectedCategoryId: string = '';

  @Input() goals: GoalModel[] = [];
  @Input() collection!: CollectionModel;
  @Input() collections: CollectionModel[] = [];
  @Input() categories: CategoryModel[] = [];
  @Input() pageParams!: PageParams;

  @Output() selectedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() deletedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() updatedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() changedSelectedCategoryEvent: EventEmitter<string> = new EventEmitter<string>();
  @Output() pageChangedEvent: EventEmitter<PageParams> = new EventEmitter<PageParams>();
  @Output() errorResponseEvent: EventEmitter<string[]> = new EventEmitter<string[]>();

  @Output() enableLoadingState: EventEmitter<any> = new EventEmitter<any>();
  @Output() disableLoadingState: EventEmitter<any> = new EventEmitter<any>();

  constructor(private goalService: GoalService){}
  ngOnChanges(changes: SimpleChanges): void {
  }

  onSelectedCategoryChange(){
    this.changedSelectedCategoryEvent.emit(this.selectedCategoryId);
  }

  handlePageChange(pageNumber: number){
    this.pageParams.selectedPage = pageNumber;
    this.pageChangedEvent.emit(this.pageParams);
  }

  onChangeGoalCompletion(goal: GoalModel, sender: any, event: MouseEvent){
    event.stopPropagation();
    goal.isCompleted = sender.target.checked;
    this.enableLoadingState.emit();
    this.goalService.update(goal.id, goal).subscribe({
      next: (result) => {
        this.disableLoadingState.emit();
        if(result.isSuccess){
          let index = this.goals.findIndex(x=>x.id === result.data.id);
          this.goals[index] = result.data;
          this.updatedGoalEvent.emit(result.data);
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        this.disableLoadingState.emit();
      }
    });
  }

  createGoal(){
    var goal: GoalModel = this.goalForm.value;
    this.enableLoadingState.emit();
    this.goalService.create(goal).subscribe({
      next: (result) =>{
        this.disableLoadingState.emit();
        if(result.isSuccess){
          let newGoal = result.data;
          newGoal.goalCategories = [];
          newGoal.attachments = [];
          this.goals.push(newGoal);
          this.isCreationMode = false;
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        this.disableLoadingState.emit();
      }
    });
  }

  deleteGoal(id: string, event: MouseEvent){
    event.stopPropagation();
    this.enableLoadingState.emit();
    this.goalService.delete(id).subscribe({
      next: (result) =>{
        this.disableLoadingState.emit();
        if(result.isSuccess){
          let index = this.goals.findIndex(x=>x.id === id);
          this.goals.splice(index, 1);
          this.deletedGoalEvent.emit(result.data);
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        this.disableLoadingState.emit();
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
