import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BootstrapFormRenderer } from '../services/bootstrap-form-renderer';
import { EventAggregator } from 'aurelia-event-aggregator';

@inject(ValidationControllerFactory,
        EventAggregator,
        HttpClient
)
export class CreateAsset {
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
        controllerFactory: ValidationControllerFactory,
        eventAggregator: EventAggregator,
        http: HttpClient
    ) {
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

                    this.eventAggregator.publish("ewFlashSuccess", "Asset is saved.")
                } else {
                    this.eventAggregator.publish("ewFlashError", "Asset is not saved.")
                }
            });
    }
}