import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { GoalsComponent } from './components/goals/goals.component';
import { CollectionsComponent } from './components/collections/collections.component';
import { GoalDetailsComponent } from './components/goals/goal-details/goal-details.component';
import { AuthComponent } from './components/auth/auth.component';
import { LocalStorageService } from './services/local-storage.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CategoriesComponent } from './components/categories/categories.component';
import { AuthorizedAccountService } from './services/implementations/authorized-account.service';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { Route, RouterModule } from '@angular/router';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AccountService } from './services/implementations/account.service';
import { AttachmentService } from './services/implementations/attachment.service';
import { CategoryService } from './services/implementations/category.service';
import { CollectionService } from './services/implementations/collection.service';
import { GoalService } from './services/implementations/goal.service';
import { NavbarTopComponent } from './components/navbar-top/navbar-top.component';

const routes: Route[] = [
  { path: 'Dashboard', component: DashboardComponent },
  { path: 'Auth', component: AuthComponent },
]

@NgModule({
  declarations: [
    AppComponent,
    GoalsComponent,
    CollectionsComponent,
    GoalDetailsComponent,
    AuthComponent,
    CategoriesComponent,
    DashboardComponent,
    NavbarTopComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [LocalStorageService, AuthorizedAccountService, AccountService, AttachmentService, CategoryService, CollectionService, GoalService, {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
