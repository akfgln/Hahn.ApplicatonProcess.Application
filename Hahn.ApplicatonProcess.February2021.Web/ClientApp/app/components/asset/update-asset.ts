import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BootstrapFormRenderer } from '../services/bootstrap-form-renderer';
import { AssetService } from '../services/asset-service'

@inject(AssetService,
    ValidationControllerFactory,
    EventAggregator,
    HttpClient
)
export class UpdateAsset {
    assetService: AssetService;
    controller: ValidationController;
    createRules: any;
    isAssetFound: boolean;
    eventAggregator: EventAggregator;
    errorMessage: string;
    http: HttpClient;
    server_side_errors: string[];

    //Asset Model
    id: number | undefined;
    assetName: string | undefined;
    department: string | undefined;
    countryOfDepartment: string | undefined;
    eMailAdressOfDepartment: string | undefined;
    purchaseDate: Date | undefined;
    isBroken: boolean | undefined;

    constructor(
        assetService: AssetService,
        controllerFactory: ValidationControllerFactory,
        eventAggregator: EventAggregator,
        http: HttpClient
    ) {
        this.assetService = assetService;
        this.controller = controllerFactory.createForCurrentScope();

        this.createRules = ValidationRules
            .ensure((a: UpdateAsset) => a.assetName).required()
            .ensure((a: UpdateAsset) => a.eMailAdressOfDepartment).required().email()
            .rules;

        this.isAssetFound = true;
        this.eventAggregator = eventAggregator;
        this.errorMessage = "";
        this.http = http;
        this.server_side_errors = [];

        this.controller.addObject(this, this.createRules);
        this.controller.addRenderer(new BootstrapFormRenderer());
        this.purchaseDate = new Date();
    }

    activate(params) {
        this.assetService.getAsset(params.id)
            .then(result => {
                
                if (typeof result === 'string' || result instanceof String)
                {
                    this.isAssetFound = false;
                    this.eventAggregator.publish("ewFlashError", result);
                    this.assetName = "";
                    this.department = "";
                    this.countryOfDepartment = "";
                    this.eMailAdressOfDepartment = "";
                    this.purchaseDate = new Date();
                    this.isBroken = false;
                    return;
                }

                if (result) {
                    this.id = result.id;
                    this.assetName = result.assetName;
                    this.department = result.department;
                    this.countryOfDepartment = result.countryOfDepartment;
                    this.eMailAdressOfDepartment = result.eMailAdressOfDepartment;
                    this.purchaseDate = result.purchaseDate;
                    this.isBroken = result.isBroken;
                }
                else {
                    this.eventAggregator.publish("ewFlashError", "An error occurred.")
                }
            }).catch(error => {
                console.log(error)
                this.eventAggregator.publish("ewFlashError", "An error occurred.")
            });
    }

    edit() {
        this.controller.validate()
            .then(result => {
                if (result.valid) {
                    this.assetService.updateAsset(this.id, {
                        id: 0,
                        assetName: this.assetName,
                        department: this.department,
                        countryOfDepartment: this.countryOfDepartment,
                        eMailAdressOfDepartment: this.eMailAdressOfDepartment,
                        purchaseDate: this.purchaseDate,
                        isBroken: this.isBroken
                    })
                        .then(result => {
                            
                            if (result.success) {
                                
                                this.eventAggregator.publish("ewFlashSuccess", "Asset is saved.")
                                this.server_side_errors = [];
                                this.isAssetFound = true;
                            }
                            else {
                                this.server_side_errors = result.errors;
                            }
                        }).catch(error => {
                            console.log(error)
                            this.eventAggregator.publish("ewFlashError", "An error occurred.")
                        });

                } else {
                    this.eventAggregator.publish("ewFlashError", "Asset is not saved.")
                }
            });
    }
}