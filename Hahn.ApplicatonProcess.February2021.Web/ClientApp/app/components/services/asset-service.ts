import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { isArray } from 'jquery';

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
                var result: any = {
                    success: false,
                    data: {},
                    errors: ["An error occured."]
                };
                console.log(error);
                return result;
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
                    data: {},
                    errors: []
                };
                if (typeof data === 'string' || data instanceof String)
                    result.errors.push(data)
                else if (!data.id) {
                    for (var key in data) {
                        var value = data[key];
                        result.errors.push(...value);
                    }
                } else {
                    result.data = data;
                    result.success = true;
                }
                return result;
            })
            .catch(error => {
                var result: any = {
                    success: false,
                    data: {},
                    errors: ["An error occured."]
                };
                console.log(error);
                return result;
            });
    }

    getAsset(id) {
        return this.http.fetch('api/Asset/' + id)
            .then(result => result.json())
            .then(data => {
                return data;
            })
            .catch(error => {
                var result: any = {
                    success: false,
                    data: {},
                    errors: ["An error occured."]
                };
                console.log(error);
                return result;
            });
    }

    deleteAsset(id) {
        return this.http.fetch('api/Asset/' + id, {
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

    updateAsset(id, asset) {
        return this.http.fetch('api/Asset/' + id, {
            method: 'put',
            body: json(asset)
        })
            .then(response => response.json())
            .then(data => {
                
                var result: any = {
                    success: false,
                    data: {},
                    errors: []
                };
                if (typeof data === 'string' || data instanceof String)
                    result.errors.push(data)
                else if (!data.id) {
                    for (var key in data) {
                        var value = data[key];
                        result.errors.push(...value);
                    }
                } else {
                    result.data = data;
                    result.success = true;
                }
                return result;
            })
            .catch(error => {
                var result: any = {
                    success: false,
                    data: {},
                    errors: ["An error occured."]
                };
                console.log(error);
                return result;
            });
    }
}