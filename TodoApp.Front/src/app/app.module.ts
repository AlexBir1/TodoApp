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
import { NgxPaginationModule } from 'ngx-pagination';
import { NotificationComponent } from './components/notification/notification.component';
import { ErrorComponent } from './components/error/error.component';
import { LoaderComponent } from './components/loader/loader.component';
import { MenuPopupComponent } from './components/menu-popup/menu-popup.component';
import { LogoutPopupComponent } from './components/logout-popup/logout-popup.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GoalNotificationComponent } from './components/goal-notification/goal-notification.component';
import { GoalNotificationsService } from './services/implementations/goal-notifications.service';

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
    NotificationComponent,
    ErrorComponent,
    LoaderComponent,
    MenuPopupComponent,
    LogoutPopupComponent,
    GoalNotificationComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    NgxPaginationModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule
  ],
  providers: [LocalStorageService, AuthorizedAccountService, AccountService, AttachmentService, CategoryService, CollectionService, GoalNotificationsService, GoalService, {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
