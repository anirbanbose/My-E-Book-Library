import { Injectable, inject } from "@angular/core";

@Injectable()
export class StorageService{
    private USER_DETAIL_KEY: string = 'User_Detail';
    private DEVICE_ID_KEY: string = 'Device_Key';

    setUserData(user: any) {
        localStorage.setItem(this.USER_DETAIL_KEY, JSON.stringify(user));
    }

    setDeviceId(deviceId: string){
        localStorage.setItem(this.DEVICE_ID_KEY, deviceId);
    }

    getLoggedInUser(): any{        
        var user = localStorage.getItem(this.USER_DETAIL_KEY);
        if(user != undefined){
            return JSON.parse(user);
        } 
        return null;
    }

    getDeviceId() : any{        
        return localStorage.getItem(this.DEVICE_ID_KEY);
    }


    removeUserData(){
        localStorage.removeItem(this.USER_DETAIL_KEY);
    }
}