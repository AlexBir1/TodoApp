import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CategoryService } from 'src/app/services/implementations/category.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { CategoryModel } from 'src/app/shared/models/category.model';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit{
  isCreationMode: boolean = false;
  categoryForm!: FormGroup;
  account!: AuthorizationModel;

  @Input() categories: CategoryModel[] = [];

  constructor(private categoryService: CategoryService, private localStorageService: LocalStorageService){}

  ngOnInit(): void {
    this.account = this.localStorageService.getAccountFromStorage();
  }

  initializeCategoryForm(){
    this.categoryForm = new FormGroup({
      colorTitle: new FormControl('', Validators.required),
      colorHex: new FormControl('', Validators.required),
    });
  }

  changeIsCreationMode(){
    this.isCreationMode = !this.isCreationMode;
    if(this.isCreationMode)
      this.initializeCategoryForm();
  }

  deleteCategory(id: string){
    this.categoryService.delete(id).subscribe({
      next: (result) => {
        if(result.isSuccess){
          let index = this.categories.findIndex(x=>x.id === id);
          this.categories.splice(index, 1);
        }
      },
      error: (error) => {

      },
    });
  }

  createCategory(){
    let category: CategoryModel = this.categoryForm.value;
    category.accountId = this.account.accountId;
    this.categoryService.create(category).subscribe({
      next: (result) => {
        if(result.isSuccess){
          this.categories.push(result.data);
          this.isCreationMode = false;
        }
      },
      error: (error) => {

      },
    });
  }
}
