import { Injectable } from "@angular/core";
import * as signalR from '@microsoft/signalR';
import { ReplaySubject } from "rxjs";
import { UserNotificationModel } from "../shared/models/user-notification.model";
import { LocalStorageService } from "./local-storage.service";

@Injectable({providedIn: 'root'})
export class SignalRService{
    hubConnection!: signalR.HubConnection;
    messageSource = new ReplaySubject<any | null>(1);
    goalNotificationSource = new ReplaySubject<UserNotificationModel | null>(1);

    constructor(private localStorageService: LocalStorageService){

    }

    startConnection = () => {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withAutomaticReconnect()
            .withUrl('https://localhost:7135/Notification', 
            {
                skipNegotiation: true, 
                transport: signalR.HttpTransportType.WebSockets,
                accessTokenFactory: () => this.localStorageService.getAccountFromStorage().token
            })
            .build();
        
        this.hubConnection.start()
        .then(ex=>{
            console.log(ex);
        })
        .catch(ex=>{
            console.log(ex);
        });
    }

    startListen(){
        this.hubConnection.on('Notify', (msg) => 
        {
            let goalNotification = msg as UserNotificationModel;
            if(goalNotification.goalId ){
                this.goalNotificationSource.next(msg);
            }
            else
                this.messageSource.next(msg)
        });
    }
}