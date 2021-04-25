import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { AssetService } from '../services/asset-service'

@inject(
    AssetService,
    EventAggregator
)
export class Asset {
    assetService: AssetService;
    assets: any[] | undefined;
    eventAggregator: EventAggregator;
    selectedId: number;
    constructor(assetService: AssetService,
        eventAggregator: EventAggregator) {
        this.assetService = assetService;
        this.assets = [];
        this.selectedId = 0;
        this.eventAggregator = eventAggregator;
        this.getList();
    }

    getList() {
        this.assetService.getAssets()
            .then(data => this.assets = data)
            .catch(err => console.log(err));
    }

    delete(asset) {
        if (confirm('Are you sure that you want to delete this asset?')) {
            this.assetService.deleteAsset(asset.id)
                .then(data => {
                    
                    if(data.success)
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