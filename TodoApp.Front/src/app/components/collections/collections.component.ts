import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
    let newCollections: CollectionModel[] = [];
    this.collections.forEach(x=>newCollections.push(x));
    let index = newCollections.findIndex(x=>x.title === 'Unsorted');
    if(index > -1)
      newCollections.splice(index, 1);
    this.collectionsExceptUnsorted = newCollections;
  }

  deleteCollection(id: string, event: MouseEvent){
    event.stopPropagation();
    this.enableLoadingState.emit();
    var collection: CollectionModel = this.collections.find(x=>x.id === id) as CollectionModel;
    if(collection.title === "All" || collection.title === "Unsorted")
      return console.log('This collection cannot be deleted.');
    this.collectionService.delete(id).subscribe({
      next: (result) => {
        this.disableLoadingState.emit();
        if(result.isSuccess){
          let index = this.collections.findIndex(x=>x.id === id);
          this.collections.splice(index, 1);
          this.deletedCollectionEvent.emit(result.data);
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        this.disableLoadingState.emit();
        console.log(error);
      }
    });
  }

  createCollection(){
    let collection: CollectionModel = this.collectionForm.value;
    collection.accountId = this.account.accountId;
    this.enableLoadingState.emit();
    this.collectionService.create(collection).subscribe({
      next: (result) => {
        this.disableLoadingState.emit();
        if(result.isSuccess){
          this.collections.push(result.data);
          this.isCreationMode = false;
        }
        else
          this.errorResponseEvent.emit(result.messages);
      },
      error: (error) => {
        this.disableLoadingState.emit();
        console.log(error);
      }
    });
  }

  selectCollection(id: string){
    this.selectedCollectionEvent.emit(this.collections.find(x=>x.id === id));
  }

  initializeCollectionForm(){
    this.collectionForm = new FormGroup({
      title: new FormControl('', Validators.required),
    });
  }
}
