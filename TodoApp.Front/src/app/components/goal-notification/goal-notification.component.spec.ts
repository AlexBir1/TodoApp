import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GoalNotificationComponent } from './goal-notification.component';

describe('GoalNotificationComponent', () => {
  let component: GoalNotificationComponent;
  let fixture: ComponentFixture<GoalNotificationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GoalNotificationComponent]
    });
    fixture = TestBed.createComponent(GoalNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
