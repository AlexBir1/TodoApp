import { trigger, state, style, transition, animate, stagger,query } from '@angular/animations';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GoalNotificationsService } from 'src/app/services/implementations/goal-notifications.service';
import { GoalService } from 'src/app/services/implementations/goal.service';
import { CategoryModel } from 'src/app/shared/models/category.model';
import { CollectionModel } from 'src/app/shared/models/collection.model';
import { GoalModel } from 'src/app/shared/models/goal.model';
import { PageParams } from 'src/app/shared/page-params';

const listAnimation = trigger('listAnimation', [
  transition(':enter', [
    style({ opacity: 0, width: '0%'}),
    animate('350ms ease-out', style({ opacity: 1, width: '100%' })),
  ]),
  transition(':leave', [
    style({ opacity: 1, width: '100%' }),
    animate('350ms ease-in', style({ opacity: 0, width: '0%' })),
  ])
]);

@Component({
  selector: 'app-goals',
  templateUrl: './goals.component.html',
  styleUrls: ['./goals.component.css'],
  animations: [listAnimation]
})
export class GoalsComponent implements OnChanges{
  isCreationMode: boolean = false;
  goalForm!: FormGroup;
  selectedCategoryId: string = '';
  submitChangeItemsPerPage: boolean = true;

  @Input() goals: GoalModel[] = [];
  @Input() collection!: CollectionModel;
  @Input() collections: CollectionModel[] = [];
  @Input() categories: CategoryModel[] = [];
  @Input() pageParams!: PageParams;

  @Output() selectedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() createdGoalEvent: EventEmitter<GoalModel> = new EventEmitter<any>();
  @Output() deletedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() updatedGoalEvent: EventEmitter<GoalModel> = new EventEmitter<GoalModel>();
  @Output() changedSelectedCategoryEvent: EventEmitter<string> = new EventEmitter<string>();
  @Output() pageChangedEvent: EventEmitter<PageParams> = new EventEmitter<PageParams>();
  @Output() errorResponseEvent: EventEmitter<string[]> = new EventEmitter<string[]>();

  @Output() enableLoadingState: EventEmitter<any> = new EventEmitter<any>();
  @Output() disableLoadingState: EventEmitter<any> = new EventEmitter<any>();

  constructor(private goalService: GoalService, private goalNotificationsService: GoalNotificationsService){
      
  }
  ngOnChanges(changes: SimpleChanges): void {
  }

  onSelectedCategoryChange(){
    this.changedSelectedCategoryEvent.emit(this.selectedCategoryId);
  }

  onItemsPerPageInputChanged(event: any){
    if(this.pageParams.itemsPerPage < 1 || this.pageParams.itemsPerPage > 10){
      this.submitChangeItemsPerPage = false;
    }
    else{
      this.submitChangeItemsPerPage = true;
    }
  }

  handlePageChange(pageNumber: number){
    this.pageParams.selectedPage = pageNumber;
    this.pageChangedEvent.emit(this.pageParams);
  }

  getUnsortedCollectionId(){
    return this.collections.find(x=>x.title === "Unsorted")!.id;
  }

  onChangeGoalCompletion(goal: GoalModel, sender: any, event: MouseEvent){
    event.stopPropagation();
    goal.isCompleted = sender.target.checked;
    this.enableLoadingState.emit();
    this.goalService.update(goal.id, goal).subscribe({
      next: (result) => {
        if(result.isSuccess){
          let index = this.goals.findIndex(x=>x.id === result.data.id);
          this.goals[index] = result.data;
          this.updatedGoalEvent.emit(result.data);
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        console.log(error);
      }
    }).add(() => this.disableLoadingState.emit());
  }

  createGoal(){
    var goal: GoalModel = this.goalForm.value;
    this.enableLoadingState.emit();
    this.goalService.create(goal).subscribe({
      next: (result) =>{
        if(result.isSuccess){
          this.goals.push(result.data);
          this.pageParams.itemsCount += 1;
          this.goalNotificationsService.create([result.data] as GoalModel[]).subscribe() 
          this.createdGoalEvent.emit();
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
      }
    }).add(() => 
    { 
      this.isCreationMode = false;
      this.disableLoadingState.emit()
    });
  }

  deleteGoal(id: string, event: MouseEvent){
    event.stopPropagation();
    this.enableLoadingState.emit();
    this.goalNotificationsService.delete(id).subscribe(()=>this.goalService.delete(id).subscribe({
      next: (result) =>{
        if(result.isSuccess){
          let index = this.goals.findIndex(x=>x.id === id);
          this.goals.splice(index, 1);
          this.pageParams.itemsCount -= 1; 
          this.deletedGoalEvent.emit(result.data);
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
      }
    })).add(() => this.disableLoadingState.emit());
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
