"use client"
import httpClient from "../utils/httpClient";

import { useState } from "react"
import { isEmpty } from "../utils/utils";
import { useRouter } from 'next/navigation'

export default function Login() {




    const renderForm = () => {

        let saida =
            <div>
                <div className="form-group">
                    <label>Usuário</label>
                    <input type="text" className="form-control" />
                </div>

                <div className="form-group">
                    <label>Senha</label>
                    <input type="text" className="form-control" />
                </div>

                <div className="form-group">
                    <button className="btn btn-primary"
                        type="button">Entrar</button>
                </div>



            </div>

        return saida;
    }


    return (
        <div className="container">
            <div className="row justify-content-center">

                <div className="col-xl-10 col-lg-12 col-md-9">

                    <div className="card o-hidden border-0 shadow-lg my-5">
                        <div className="card-body p-0">
                            <div className="row">
                                <div className="col-lg-6 d-none d-lg-block bg-login-image"></div>
                                <div className="col-lg-6">
                                    <div className="p-5">
                                        <div className="text-center">
                                            <h1 className="h4 text-gray-900 mb-4">Autenticação</h1>
                                        </div>
                                        {renderForm()}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

            </div>

        </div>
    )
}