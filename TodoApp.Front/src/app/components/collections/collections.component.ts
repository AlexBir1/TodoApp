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
  @Output() selectedCollectionEvent: EventEmitter<CollectionModel> = new EventEmitter<CollectionModel>();
  @Output() deletedCollectionEvent: EventEmitter<CollectionModel> = new EventEmitter<CollectionModel>();

  constructor(private collectionService: CollectionService, private localStorageService: LocalStorageService){}

  ngOnInit(){
    this.account = this.localStorageService.getAccountFromStorage();
  }

  changeIsCreationMode(){
    this.isCreationMode = !this.isCreationMode;
    if(this.isCreationMode)
      this.initializeCollectionForm();
  }

  deleteCollection(id: string, event: MouseEvent){
    event.stopPropagation();
    var collection: CollectionModel = this.collections.find(x=>x.id === id) as CollectionModel;
    if(collection.title === "All" || collection.title === "Unsorted")
      return console.log('This collection cannot be deleted.');
    this.collectionService.delete(id).subscribe({
      next: (result) => {
        if(result.isSuccess){
          let index = this.collections.findIndex(x=>x.id === id);
          this.collections.splice(index, 1);
          this.deletedCollectionEvent.emit(result.data);
        }
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  createCollection(){
    let collection: CollectionModel = this.collectionForm.value;
    collection.accountId = this.account.accountId;
    this.collectionService.create(collection).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.collections.push(result.data);
          this.isCreationMode = false;
        }
      },
      error: (error) => {
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
