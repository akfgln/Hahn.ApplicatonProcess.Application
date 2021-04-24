import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { AuthService } from '../services/auth-service';
import { EventAggregator } from 'aurelia-event-aggregator';

@inject(AuthService, ValidationControllerFactory, EventAggregator, Router)
export class Login {
    authService: AuthService;
    controller: ValidationController;
    email: string;
    eventAggregator: EventAggregator;
    errorMessage: string;
    password: string;
    server_side_errors: string[];
    router: Router;

    constructor(
        authService: AuthService,
        controllerFactory: ValidationControllerFactory,
        eventAggregator: EventAggregator,
        router: Router
    ) {
        this.authService = authService;
        this.controller = controllerFactory.createForCurrentScope();
        this.email = "";
        this.eventAggregator = eventAggregator;
        this.errorMessage = "";
        this.password = "";
        this.server_side_errors = [];
        this.router = router;

        var loginRules = ValidationRules
            .ensure((a: Login) => a.password).required()
            .ensure((a: Login) => a.email).required().email()
            .rules;
        this.controller.addObject(this, loginRules);
    }

    logIn() {
        this.controller.validate()
            .then(result => {
                if (result.valid) {
                    this.authService.logIn(this.email,
                        this.password)
                        .then(tokenResult => {
                            if (tokenResult.success) {
                                this.server_side_errors = [];
                                this.router.navigateToRoute('home');
                            }
                            else {
                                this.server_side_errors = tokenResult.errors;
                            }
                        });
                }
            });
    }
}
