import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class UpdateAsset {
    http: HttpClient;

    constructor(http: HttpClient) {
        this.http = http;
    }
}
