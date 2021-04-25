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
                debugger;
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
                    this.assetName = result.assetName;
                    this.department = result.department;
                    this.countryOfDepartment = result.countryOfDepartment;
                    this.eMailAdressOfDepartment = result.eMailAdressOfDepartment;
                    this.purchaseDate = result.purchaseDate;
                    this.isBroken = result.isBroken;
                }
                else {
                    this.eventAggregator.publish("ewFlashError", "An error occured.")
                }
            }).catch(error => {
                console.log(error)
                this.eventAggregator.publish("ewFlashError", "An error occured.")
            });
    }

    addNew() {
        this.controller.validate()
            .then(result => {
                if (result.valid) {
                    this.assetService.createAsset({
                        id: 0,
                        assetName: this.assetName,
                        department: this.department,
                        countryOfDepartment: this.countryOfDepartment,
                        eMailAdressOfDepartment: this.eMailAdressOfDepartment,
                        purchaseDate: this.purchaseDate,
                        isBroken: this.isBroken
                    })
                        .then(result => {
                            debugger;
                            if (result.success) {
                                debugger;
                                this.eventAggregator.publish("ewFlashSuccess", "Asset is saved.")
                                this.server_side_errors = [];

                                this.assetName = "";
                                this.department = "";
                                this.countryOfDepartment = "";
                                this.eMailAdressOfDepartment = "";
                                this.purchaseDate = new Date();
                                this.isBroken = false;

                            }
                            else {
                                this.server_side_errors = result.errors;
                            }
                        }).catch(error => {
                            console.log(error)
                            this.eventAggregator.publish("ewFlashError", "An error occured.")
                        });

                } else {
                    this.eventAggregator.publish("ewFlashError", "Asset is not saved.")
                }
            });
    }
}