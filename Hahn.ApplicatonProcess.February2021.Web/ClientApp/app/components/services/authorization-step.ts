import { Redirect, NavigationInstruction } from 'aurelia-router';
import { EventAggregator } from 'aurelia-event-aggregator';
import { AuthService } from "./auth-service";

export class AuthorizeStep {
    authService: AuthService;
    eventAggregator: EventAggregator;

    constructor(authService: AuthService,
        eventAggregator: EventAggregator) {
        this.authService = authService;
        this.eventAggregator = eventAggregator;
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

