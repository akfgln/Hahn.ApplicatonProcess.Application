import { Aurelia, PLATFORM } from 'aurelia-framework';
import { inject } from "aurelia-framework";
import { Router, RouterConfiguration } from 'aurelia-router';
import { HttpClient } from "aurelia-fetch-client";
import { AuthService } from "../services/auth-service";
import { AuthorizeStep } from "../services/authorization-step";
import { FlashMessageService } from '../services/flash-message-service';

@inject(AuthService, HttpClient, FlashMessageService)
export class App {
    authService: AuthService;
    router: Router | undefined;
    flashMessageService: FlashMessageService;

    constructor(authService: AuthService, http: HttpClient
        , flashMessageService: FlashMessageService
    ) {
        this.authService = authService;
        this.flashMessageService = flashMessageService;
        http.configure(config => {
            config
                .withBaseUrl('/')
                .withInterceptor(this.authService.tokenInterceptor);
        });
    }

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Hahn Applicaton';
        config.map([{
            route: ['', 'home'],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: PLATFORM.moduleName('../home/home'),
            nav: true,
            title: 'Home'
        }, {
            route: 'counter',
            name: 'counter',
            settings: { icon: 'education' },
            moduleId: PLATFORM.moduleName('../counter/counter'),
            nav: true,
            title: 'Counter'
        }, {
            route: 'asset',
            name: 'asset',
            settings: { icon: 'th-list' },
            moduleId: PLATFORM.moduleName('../asset/asset'),
            nav: true,
            title: 'Assets'
        },
        {
            route: "login",
            name: "login",
            settings: { icon: 'user' },
            moduleId: PLATFORM.moduleName('../login/login'),
            title: "Login",
            nav: true,
        }]);

        this.router = router;

        let step = new AuthorizeStep(this.authService);

        config.addAuthorizeStep(step);

    }
}