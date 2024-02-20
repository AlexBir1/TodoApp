import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CollectionService } from 'src/app/services/implementations/collection.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { CollectionModel } from 'src/app/shared/models/collection.model';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.css']
})
export class CollectionsComponent implements OnInit{
  isCreationMode: boolean = false;
  account!: AuthorizationModel;
  collectionForm!: FormGroup;

  @Input() collections: CollectionModel[] = [];
  collectionsExceptUnsorted: CollectionModel[] = [];
  @Output() selectedCollectionEvent: EventEmitter<CollectionModel> = new EventEmitter<CollectionModel>();
  @Output() createdCollectionEvent: EventEmitter<CollectionModel> = new EventEmitter<CollectionModel>();
  @Output() deletedCollectionEvent: EventEmitter<CollectionModel> = new EventEmitter<CollectionModel>();

  @Output() enableLoadingState: EventEmitter<any> = new EventEmitter<any>();
  @Output() disableLoadingState: EventEmitter<any> = new EventEmitter<any>();

  @Output() errorResponseEvent: EventEmitter<string[]> = new EventEmitter<string[]>();

  constructor(private collectionService: CollectionService, private localStorageService: LocalStorageService){}

  ngOnInit(){
    this.account = this.localStorageService.getAccountFromStorage();
    this.setCollectionsExceptUnsorted();
  }

  changeIsCreationMode(){
    this.isCreationMode = !this.isCreationMode;
    if(this.isCreationMode)
      this.initializeCollectionForm();
  }

  getUnsortedCollectionId(){
    let collection = this.collections!.find(x=>x.title === 'Unsorted');
    return collection!.id;
  }

  setCollectionsExceptUnsorted(){
    this.collectionsExceptUnsorted = [];
    this.collections.forEach(x=>
      {
        if(x.title != 'Unsorted') 
          this.collectionsExceptUnsorted.push(x)
      });
  }

  deleteCollection(id: string, event: MouseEvent){
    event.stopPropagation();
    this.enableLoadingState.emit();
    var collection: CollectionModel = this.collections.find(x=>x.id === id) as CollectionModel;
    if(collection.title === "All" || collection.title === "Unsorted")
      return console.log('This collection cannot be deleted.');
    this.collectionService.delete(id).subscribe({
      next: (result) => {
        if(result.isSuccess){
          let index = this.collections.findIndex(x=>x.id === id);
          this.collections.splice(index,1);
          this.setCollectionsExceptUnsorted();
          this.deletedCollectionEvent.emit(result.data);
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        console.log(error);
      }
    }).add(() => this.disableLoadingState.emit());
  }

  createCollection(){
    let collection: CollectionModel = this.collectionForm.value;
    collection.accountId = this.account.accountId;
    this.enableLoadingState.emit();
    this.collectionService.create(collection).subscribe({
      next: (result) => {
        if(result.isSuccess){
          let newCollection = result.data;
          this.collections.push(newCollection);
          this.setCollectionsExceptUnsorted();
          this.isCreationMode = false;
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        console.log(error);
      }
    }).add(() => this.disableLoadingState.emit());
  }

  selectCollection(id: string){
    this.selectedCollectionEvent.emit(this.collections.find(x=>x.id === id));
  }

  initializeCollectionForm(){
    this.collectionForm = new FormGroup({
      title: new FormControl(null, Validators.required),
    });
  }
}
