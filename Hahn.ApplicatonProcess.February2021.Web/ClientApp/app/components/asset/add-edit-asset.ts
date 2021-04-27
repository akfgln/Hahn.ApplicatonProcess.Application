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
    countries: any[];
    createRules: any;
    departments: any[];
    eventAggregator: EventAggregator;
    errorMessage: string;
    http: HttpClient;
    ifNewAsset: boolean;
    isAssetFound: boolean;
    isFormNotValid: boolean;
    isFormSending: boolean;
    isShowError: boolean;
    server_side_errors: string[];

    //Asset Model
    id: number | undefined;
    assetName: string | undefined;
    department: string | undefined;
    countryOfDepartment: string | undefined;
    eMailAdressOfDepartment: string | undefined;
    purchaseDate: Date;
    isBroken: boolean;

    selectOptions = {
        liveSearch: true,
        showSubtext: true,
        showTick: true,
        selectedTextFormat: 'count > 3'
    };

    constructor(
        assetService: AssetService,
        controllerFactory: ValidationControllerFactory,
        eventAggregator: EventAggregator,
        http: HttpClient
    ) {
        this.assetService = assetService;
        this.controller = controllerFactory.createForCurrentScope();
        this.countries = [];
        ValidationRules.customRule(
            'purchaseDate',
            (value, obj, max) => {
                
                var d = new Date(value);
                return value === null
                    || value === undefined
                    || value.trim() === ''
                    || d <= max
            },
            `\${$displayName} must be a Date and must not be older then one year.`,
            (max) => ({ max })
        );

        var nextYear = new Date(new Date().setFullYear(new Date().getFullYear() + 1));
        nextYear.setHours(0, 0, 0, 0);
        this.createRules = ValidationRules
            .ensure((a: CreateAsset) => a.assetName).required().minLength(5)
            .ensure((a: CreateAsset) => a.eMailAdressOfDepartment).required().email()
            .ensure((a: CreateAsset) => a.purchaseDate).required().satisfiesRule("purchaseDate", nextYear)
            .rules;

        this.departments = [];
        this.eventAggregator = eventAggregator;
        this.errorMessage = "";
        this.http = http;
        this.isAssetFound = true;
        this.ifNewAsset = true;
        this.isFormNotValid = false;
        this.isFormSending = false;
        this.isShowError = false;
        this.isBroken = false;
        this.server_side_errors = [];

        this.controller.addObject(this, this.createRules);
        this.controller.addRenderer(new BootstrapFormRenderer());
        this.purchaseDate = new Date();
        this.checkFormValid();
    }


    activate(params) {

        this.ifNewAsset = params.id == 0;
        if (!this.ifNewAsset)
            this.assetService.getAsset(params.id)
                .then(result => {
                    if (typeof result === 'string' || result instanceof String) {
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
                        this.eMailAdressOfDepartment = result.eMailAdressOfDepartment;
                        this.purchaseDate = result.purchaseDate;
                        this.isBroken = result.isBroken;
                        this.getCountries(result.countryOfDepartment);
                        this.getDepartments(result.department);
                    }
                    else {
                        this.eventAggregator.publish("ewFlashError", "An error occurred.")
                    }
                }).catch(error => {
                    console.log(error)
                    this.eventAggregator.publish("ewFlashError", "An error occurred.")
                });
        else {
            this.getCountries("");
            this.getDepartments("");
        }
    }

    save() {
        this.isShowError = true;
        this.isFormSending = true;
        if (this.ifNewAsset) {
            this.addNew();
        } else {
            this.edit();
        }
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
                            this.isFormSending = false;
                            if (result.success) {
                                this.server_side_errors = [];
                                //this.resetObject();
                                this.eventAggregator.publish("ewFlashSuccess", "Asset is saved.")
                            }
                            else {
                                this.server_side_errors = result.errors;
                            }
                        }).catch(error => {
                            this.isFormSending = false;
                            console.log(error)
                            this.eventAggregator.publish("ewFlashError", "An error occurred.")
                        });

                } else {
                    this.isFormSending = false;
                    this.eventAggregator.publish("ewFlashError", "Asset is not saved.")
                }
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
                            this.isFormSending = false;
                            if (result.success) {
                                this.eventAggregator.publish("ewFlashSuccess", "Asset is saved.")
                                this.server_side_errors = [];
                                this.isAssetFound = true;
                            }
                            else {
                                this.server_side_errors = result.errors;
                            }
                        }).catch(error => {
                            this.isFormSending = false;
                            console.log(error)
                            this.eventAggregator.publish("ewFlashError", "An error occurred.")
                        });

                } else {
                    this.isFormSending = false;
                    this.eventAggregator.publish("ewFlashError", "Asset is not saved.")
                }
            });
    }

    getCountries(countryOfDepartment) {
        this.assetService.getCountries()
            .then(result => {
                if (result.success) {
                    this.countries = [];
                    for (var i = 0; i < result.data.length; i++) {
                        let country = result.data[i];
                        this.countries.push({ id: country.name, option: country.name });
                    }
                    if (!this.ifNewAsset)
                        this.countryOfDepartment = countryOfDepartment;
                }
                else
                    this.eventAggregator.publish("ewFlashError", result.errors.join("\n"));
            })
            .catch(error => {
                console.log(error)
                this.eventAggregator.publish("ewFlashError", "An error occurred.")
            });
    }

    getDepartments(department) {
        this.assetService.getDepartments()
            .then(result => {
                if (result.success) {
                    this.departments = [];
                    for (var i = 0; i < result.data.length; i++) {
                        let department = result.data[i];
                        this.departments.push({ id: department, option: department });
                    }
                    if (!this.ifNewAsset)
                        this.department = department;
                }
                else
                    this.eventAggregator.publish("ewFlashError", result.errors.join("\n"));
            })
            .catch(error => {
                console.log(error)
                this.eventAggregator.publish("ewFlashError", "An error occurred.")
            });
    }

    checkFormValid() {
        this.isShowError = false;
        this.controller.validate()
            .then(result => {
                if (result.valid) {
                    this.isFormNotValid = false;
                } else {
                    this.isFormNotValid = true;
                }
            });
    }

    resetForm() {
        if (confirm('Are you sure that you want to reset the from?')) {
            this.resetObject();
            this.checkFormValid();
        }
    }

    private resetObject() {
        this.assetName = "";
        this.department = "";
        this.countryOfDepartment = "";
        this.eMailAdressOfDepartment = "";
        this.purchaseDate = new Date();
        this.isBroken = false;
    }

}