import { inject } from 'aurelia-framework';
import { AssetService } from '../services/asset-service'

@inject(AssetService)
export class Asset {
    assetService: AssetService | undefined;
    assets: any[] | undefined;
    selectedId: number;
    constructor(assetService: AssetService) {
        this.assetService = assetService;
        this.assets = [];
        this.selectedId = 0;
        this.assetService.getAssets()
            .then(data => this.assets = data)
            .catch(err => console.log(err));
    }

    getDetails(asset) {
        this.selectedId = asset.id;
        return true;
    }

}