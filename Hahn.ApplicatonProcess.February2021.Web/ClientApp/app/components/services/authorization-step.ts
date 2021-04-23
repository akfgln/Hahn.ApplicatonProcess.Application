import { Redirect, NavigationInstruction } from 'aurelia-router';
import { AuthService } from "./auth-service";

export class AuthorizeStep {
    authService: AuthService;

    constructor(authService: AuthService) {
        this.authService = authService;
    }

    run(navigationInstruction: any, next: any) {
        if (navigationInstruction.getAllInstructions()
            .some((i: NavigationInstruction) => i.config.settings.auth)) {

            if (!this.authService.isLoggedIn()) {
                return next.cancel(new Redirect('login'));
            }
        }

        return next();
    }
}

