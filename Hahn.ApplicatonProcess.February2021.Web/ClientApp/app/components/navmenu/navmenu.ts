import {inject} from 'aurelia-framework';
import {AuthService} from '../services/auth-service';
import {Router} from 'aurelia-router';

@inject(Router,AuthService)
export class NavMenu{
    user: any;
    router: Router;
    authService: AuthService;

    constructor(router:Router, authService:AuthService){
        this.authService = authService;
        this.router = router;
    }

    bind(){
        this.user = this.authService.getUser();
    }

    logOut(){
       this.authService.logOut();
       this.router.navigateToRoute('login');
    }
}

