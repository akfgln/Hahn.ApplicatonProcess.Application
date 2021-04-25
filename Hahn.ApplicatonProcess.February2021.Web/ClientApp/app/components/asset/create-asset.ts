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
export class CreateAsset {
    assetService: AssetService;
    controller: ValidationController;
    createRules: any;
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
            .ensure((a: CreateAsset) => a.assetName).required()
            .ensure((a: CreateAsset) => a.eMailAdressOfDepartment).required().email()
            .rules;

        this.eventAggregator = eventAggregator;
        this.errorMessage = "";
        this.http = http;
        this.server_side_errors = [];

        this.controller.addObject(this, this.createRules);
        this.controller.addRenderer(new BootstrapFormRenderer());
        this.purchaseDate = new Date();
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
                            
                            if (result.success) {
                                
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