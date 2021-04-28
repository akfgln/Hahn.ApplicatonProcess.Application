import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { AssetService } from '../services/asset-service'
import { AuthService } from '../services/auth-service';

@inject(
    AuthService,
    AssetService,
    EventAggregator
)
export class Asset {
    authService: AuthService;
    assetService: AssetService;
    assets: any[];
    eventAggregator: EventAggregator;
    selectedId: number;
    isLogin: boolean;
    constructor(
        authService: AuthService,
        assetService: AssetService,
        eventAggregator: EventAggregator) {
        this.authService = authService;
        this.assetService = assetService;
        this.assets = [];
        this.selectedId = 0;
        this.eventAggregator = eventAggregator;
        this.isLogin = this.authService.isLoggedIn();
        this.getList();
    }

    getList() {
        if (this.isLogin)
            this.assetService.getAssets()
                .then(data => {
                    debugger;
                    if (!data.errors)
                        this.assets = data;
                    else {
                        this.eventAggregator.publish("ewFlashError", data.errors.join("\n"));
                    }
                })
                .catch(err => console.log(err));
    }

    delete(asset) {
        if (confirm('Are you sure that you want to delete this asset?')) {
            this.assetService.deleteAsset(asset.id)
                .then(data => {

                    if (data.success)
                        this.eventAggregator.publish("ewFlashSuccess", "Asset is deleted.");
                    else
                        this.eventAggregator.publish("ewFlashError", data.errors.join("\n"));

                    this.getList();
                })
                .catch(error => {
                    console.log(error)
                    this.eventAggregator.publish("ewFlashError", "An error occurred.")
                });
        }
    }
}