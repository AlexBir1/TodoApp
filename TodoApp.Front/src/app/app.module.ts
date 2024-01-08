import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { GoalsComponent } from './components/goals/goals.component';
import { CollectionsComponent } from './components/collections/collections.component';
import { GoalDetailsComponent } from './components/goals/goal-details/goal-details.component';
import { AuthComponent } from './components/auth/auth.component';
import { LocalStorageService } from './services/local-storage.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    GoalsComponent,
    CollectionsComponent,
    GoalDetailsComponent,
    AuthComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
  ],
  providers: [LocalStorageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
