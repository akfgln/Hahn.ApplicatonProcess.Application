import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class CreateAsset {
    http: HttpClient;
    asset: AssetModel;

    constructor(http: HttpClient) {
        this.http = http;
        this.asset = new AssetModel();
    }

    addNew() {

    }
}

class AssetModel {
    id: number | undefined;
    assetName: string | undefined;
    department: string | undefined;
    countryOfDepartment: string | undefined;
    eMailAdressOfDepartment: string | undefined;
    purchaseDate: Date | undefined;
    isBroken: boolean | undefined;
    constructor() {
        this.purchaseDate = new Date();
    }
}