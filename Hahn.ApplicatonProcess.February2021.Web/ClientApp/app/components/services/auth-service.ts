import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class AuthService {

    http: HttpClient;
    constructor(http: HttpClient) {
        this.http = http;
    }

    logIn(userName: string, password: string) {
        return this.http.fetch('api/Login/Authenticate', {
            method: 'post',
            body: json({ email: userName, password: password })
        })
            .then(response => response.json())
            .then(tokenResult => {
                var result: any = {
                    success: false,
                    token: "",
                    errors: []
                };
                
                if (typeof tokenResult === 'string' || tokenResult instanceof String)
                    result.errors.push(tokenResult)
                else {
                    if (tokenResult.Email) {
                        result.errors.push(...tokenResult.Email);
                    }
                    if (tokenResult.Password) {
                        result.errors.push(...tokenResult.Password);
                    }

                }
                if (tokenResult.token) {
                    result.success = true
                    window.localStorage.setItem("token", tokenResult.token);
                } else {
                    result.errors.push("Authentication is failed.")
                }
                return result;
            })
            .catch(error => {
                
                let tokenResult = {
                    success: false,
                    message: error.message
                };
                return tokenResult;
            });
    }

    logOut() {
        window.localStorage.removeItem("token");
    }



    isLoggedIn() {
        let token = this.getToken();

        if (token) return true;

        return false;
    }

    getToken() {
        return window.localStorage.getItem("token");
    }

    getUser() {
        let token = this.decodeToken(this.getToken());
        return token;
    }

    decodeToken(token: any) {

        token = token || this.getToken();

        if (!token) return;

        try {
            return JSON.parse(atob(token.split('.')[1]));
        }
        catch (e) {
            return null;
        }
    }

    get tokenInterceptor() {
        let auth = this;
        return {
            request(request: any) {
                
                let token = auth.getToken();
                if (token) {
                    request.headers
                        .append('authorization', `${token}`);
                }
                return request;
            }
        };
    }
}