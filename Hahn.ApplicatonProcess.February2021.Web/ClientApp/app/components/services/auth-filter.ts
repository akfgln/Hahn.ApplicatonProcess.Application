import { AuthService } from './auth-service';
import { inject } from 'aurelia-framework';
import { NavigationInstruction } from 'aurelia-router';

@inject(AuthService)
export class AuthFilterValueConverter {
    authService: AuthService;

    constructor(authService: AuthService) {
        this.authService = authService;
    }

    toView(routes: any) {
        
        let isAuthenticated = this.authService.isLoggedIn();
        //let isAdmin = isAuthenticated && this.authService.getUser() != null && this.authService.getUser().admin;

        return routes.filter((r: NavigationInstruction) => r.config.settings.auth === undefined
            || (r.config.settings.auth === isAuthenticated
                //&& (!r.config.settings.admin || isAdmin)
            ));
    }
}

