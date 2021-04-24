import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class Asset {
    public assets: AssetModel[] | undefined;

    constructor(http: HttpClient) {
        http.fetch('api/Asset')
            .then(result => result.json())
            .then(data => {
                debugger;
                this.assets = data;
            })
            .catch(error => {
                debugger;
                let tokenResult = {
                    success: false,
                    message: error.message
                };
                return tokenResult;
            });
    }
}

interface AssetModel {
    id: number;
    assetName: string;
    department: string;
    countryOfDepartment: string;
    eMailAdressOfDepartment: string;
    purchaseDate: Date;
    isBroken: boolean;
}