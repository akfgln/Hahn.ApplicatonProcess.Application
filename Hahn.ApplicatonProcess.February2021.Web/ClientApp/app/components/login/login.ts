import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { HttpClient } from "aurelia-fetch-client";
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BootstrapFormRenderer } from '../services/bootstrap-form-renderer';
import { EventAggregator } from 'aurelia-event-aggregator';
import { AuthService } from '../services/auth-service';

@inject(AuthService, HttpClient, ValidationControllerFactory,
    EventAggregator, Router)
export class Login {
    authService: AuthService;
    controller: ValidationController;
    email: string;
    eventAggregator: EventAggregator;
    errorMessage: string;
    http: HttpClient;
    loginRules: any;
    password: string;
    server_side_errors: string[];
    router: Router;

    constructor(
        authService: AuthService,
        http: HttpClient,
        controllerFactory: ValidationControllerFactory,
        eventAggregator: EventAggregator,
        router: Router
    ) {
        this.authService = authService;
        this.controller = controllerFactory.createForCurrentScope();
        this.email = "admin@hahn.com";
        this.eventAggregator = eventAggregator;
        this.errorMessage = "";
        this.http = http;
        this.password = "admin";
        this.server_side_errors = [];
        this.router = router;

        this.loginRules = ValidationRules
            .ensure((a: Login) => a.password).required()
            .ensure((a: Login) => a.email).required().email()
            .rules;
        this.controller.addObject(this, this.loginRules);

        this.controller.addRenderer(new BootstrapFormRenderer());
    }

    logIn() {
        this.controller.validate()
            .then(result => {
                debugger;
                if (result.valid) {
                    this.authService.logIn(this.email,
                        this.password)
                        .then(tokenResult => {
                            if (tokenResult.success) {
                                debugger;
                                this.eventAggregator.publish("ewFlashSuccess", "Authentication is completed.")
                                this.server_side_errors = [];

                                this.router.navigateToRoute('home',
                                    this.router.currentInstruction.params,
                                    { replace: true });
                            }
                            else {
                                this.server_side_errors = tokenResult.errors;
                            }
                        });
                } else {
                    this.eventAggregator.publish("ewFlashError", "Authentication is failed.")
                }
            });
    }
}
