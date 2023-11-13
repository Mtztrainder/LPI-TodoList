"use client"

import { useEffect, useState } from "react"
import { gerarId, isEmpty } from "../../utils/utils";
import httpClient from "../../utils/httpClient";
import LoadingIndicator from "@/app/components/loadingIndicador/LoadingIndicator";

export default function TodoList() {

    const [stateItem, setStateItem] = useState({
        id: "",
        descricao: ""
    })

    const [stateMsg, setStateMsg] = useState(null);

    const [stateItens, setStateItens] = useState([]);
    const [stateItensSelecionados, setStateItensSelecionados] = useState([]);
    const [aguarde, setAguarde] = useState(true);


    useEffect(() =>{
        obterTarefas()
    }, [])

    const obterTarefas = () => {

        setAguarde(true)
        httpClient.get("Todo/ObterTodos")
        .then(r => r.json())
        .then(r => {
            setStateItens(r.data)
        })
        .finally(() =>{
            setAguarde(false)
        })
    }

    const excluir = (item) => {
        if (!confirm(`Excluir: ${item.descricao}`)){
            return;
        }

        setAguarde(true);

        httpClient.delete(`Todo/Excluir?id=${item.id}`)
        .then(r => r.json)
        .then(r => {
            obterTarefas();
        })
        .finally(() => {
            setAguarde(false);
        })
    }

    const editar = (itemEditar) => {
        setStateItem({
            id: itemEditar.id,
            descricao: itemEditar.descricao
        })
    }

    const adicionar = () => {
        if (isEmpty(stateItem.descricao)){
            setStateMsg({
                tipo: "danger",
                texto: "Tarefa não informada."
            })
            return;
        }

        setAguarde(true);

        let dados = {
            id: stateItem.id,
            descricao: stateItem.descricao
        }

        let p = null

        if (dados.id == "")
            p = httpClient.post("Todo/Adicionar", dados);
        else
            p = httpClient.put(`Todo/Atualizar?id=${dados.id}`, dados);

        p.then(r => r.json())
        .then(r => {

            if (r.success)
            {
                setStateItem({id: "", descricao: ""});
                obterTarefas();
                setStateMsg({
                    tipo: "success",
                    texto: "Tarefa adicionada com sucesso."
                });
            }
            else
            {
                setStateMsg({
                    tipo: "danger",
                    texto: r.msg
                });
            }

        })
        .finally(() => {
            setAguarde(false);
        })
    }

    const selecionar = (item, selecionado) => {
        let itens = [... stateItensSelecionados];

        if (selecionado)
        {
            itens.push(item.id);
            setStateItensSelecionados(itens);
        }
        else
        {
            let novosItens = itens.filter(id => id != item.id)
            setStateItensSelecionados(novosItens);
        }
    }

    const ExcluirSelecionados = () => {
        setStateItensSelecionados([])
    }

    const renderForm = () => {
        let saida = 
        <div>
            <div className="form-group">
                <label>Tarefa</label>
                <input type="text" 
                    className="form-control" 
                    value={stateItem.descricao}
                    onChange={(e) => {
                        e.preventDefault()
                        setStateItem(prevState => ({
                            //utilizado para clonar todos os atributos anteriores e posteriormente altera os definidos
                            ...prevState,
                            descricao: e.target.value
                        }))
                    }} />
            </div>

            {!isEmpty(stateMsg) && !isEmpty(stateMsg.tipo) &&
                <div className={"alert alert-"+stateMsg.tipo}>{stateMsg.texto}</div>
            }

            <div className="form-group">
                <button className="btn btn-primary" 
                        type="button"
                        onClick={adicionar}>
                    {stateItem.id == "" ? "Adicionar" : "Salvar"}
                </button>
            </div>

        </div>

        return saida;
    }

    const renderItens = () => {
        let saida = 
        <div>
            <div>
                Tarefas: {stateItens.length}
            </div>
            
            <table className="table">
                <thead>
                    <tr>
                        <th>Tarefas</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>
                    {stateItens.map((item, index) => {
                        let editando = item.id == stateItem.id
                        // let isSelecionado = stateItensSelecionados.filter(e => e.id == item.id)

                        return (
                            <tr 
                                key={"tarefa-"+index}
                                style={!editando ? {opacity: 0.5} : null} >

                                <td>
                                    <input type="checkbox" className="mr-2" 
                                            onChange={(e) => selecionar(item, e.target.checked)}
                                    />
                                    {item.descricao}
                                </td>
                                <td>
                                    {!editando &&
                                        <>
                                            <a onClick={() => excluir(item)} style={{marginRight: 15}}>
                                                <i class="fas fa-trash"></i>
                                            </a>
                                            <a onClick={() => editar(item)}>
                                                <i class="fas fa-edit"></i>
                                            </a>
                                        </>
                                    }
                                </td>
                            </tr>
                        )
                    })}
                </tbody>
            </table>

            {stateItensSelecionados.length > 0 &&
                <button type="button" className="btn btn-primary" 
                    onClick={e => ExcluirSelecionados()}>
                    Excluir Selecionados ({stateItensSelecionados.length})
                </button>
            }
        </div>

        return saida;
    }


    return (
        <div>
            <h1>Todo List API</h1>
            {renderForm()}
            {renderItens()}
            {aguarde && <LoadingIndicator full/>}
        </div>
    );
}