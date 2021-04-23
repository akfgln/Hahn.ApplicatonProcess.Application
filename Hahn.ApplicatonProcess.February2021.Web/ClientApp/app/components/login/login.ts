import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
//import { ValidationController, ValidationControllerFactory, ValidationRules } from 'aurelia-validation';
import { AuthService } from '../services/auth-service';
import { EventAggregator } from 'aurelia-event-aggregator';

@inject(Router, //ValidationControllerFactory,
    AuthService, EventAggregator)
export class Login {
    eventAggregator: EventAggregator;
    errorMessage: string;
    password: string;
    email: string;
    authService: AuthService;
    router: Router;
    //controller: ValidationController;

    constructor(router: Router, authService: AuthService//, controllerFactory: ValidationControllerFactory
    ,eventAggregator: EventAggregator ) {
        this.authService = authService;
        this.router = router;
        this.errorMessage = "";
        this.password = "";
        this.email = "";
this.eventAggregator = eventAggregator;
        //this.controller = controllerFactory.createForCurrentScope();
    }

    logIn() {
        this.authService.logIn(this.email,
            this.password)
            .then(tokenResult => {
                if (tokenResult.success) {
                    this.errorMessage = "";
                    this.router.navigateToRoute('home');
                }
                else {
                    this.errorMessage = tokenResult.message;
                }
            });
    }
}

//ValidationRules
//    .ensure((a:Login) => a.password).required()
//    .ensure((a:Login) => a.email).required().email()
//    .on(Login);


