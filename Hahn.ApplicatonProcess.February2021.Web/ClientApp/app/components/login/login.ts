import { inject } from 'aurelia-framework';
import { AuthService } from '../services/auth-service';
import { Router } from 'aurelia-router';

@inject(Router, AuthService)
export class Login {
    errorMessage: string;
    password: string;
    email: string;
    authService: AuthService;
    router: Router;

    constructor(router: Router, authService: AuthService) {
        this.authService = authService;
        this.router = router;
        this.errorMessage = "";
        this.password = "";
        this.email = "";
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

