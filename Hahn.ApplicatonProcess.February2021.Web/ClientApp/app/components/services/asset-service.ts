import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class AssetService {

    http: HttpClient;
    constructor(http: HttpClient) {
        this.http = http;
    }

    getAssets() {

        return this.http.fetch('api/Asset')
            .then(result => result.json())
            .then(data => {
                return data;
            })
            .catch(error => {
                //TODO message
                console.log(error);
                return;
            });
    }

    createAsset(asset) {
        return this.http.fetch('api/Asset', {
            method: 'post',
            body: json(asset)
        })
            .then(response => response.json())
            .then(data => {
                var result: any = {
                    success: false,
                    errors: []
                };
               
                return result;
            })
            .catch(error => {

                //TODO message
                console.log(error);
                return;
            });
    }

    getAsset(id) {
        return this.http.fetch('api/Asset/'+ id)
            .then(result => result.json())
            .then(data => {
                return data;
            })
            .catch(error => {
                //TODO message
                console.log(error);
                return;
            });
    }

    deleteAsset(id) {
        return this.http.fetch('api/Asset/'+id, {
            method: 'delete'
        })
            .then(response => response.json())
            .then(data => {
                var result: any = {
                    success: false,
                    errors: []
                };

                return result;
            })
            .catch(error => {

                //TODO message
                console.log(error);
                return;
            });
    }

    updateClient(id, asset) {
        return this.http.fetch('api/Asset/'+id, {
            method: 'put',
            body: json(asset)
        })
            .then(response => response.json())
            .then(data => {
                var result: any = {
                    success: false,
                    errors: []
                };

                return result;
            })
            .catch(error => {

                //TODO message
                console.log(error);
                return;
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