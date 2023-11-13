"use client"

import { useState } from "react"
import { gerarId, isEmpty } from "../../utils/utils";

export default function TodoList() {

    const [stateItem, setStateItem] = useState({
        id: "",
        descricao: ""
    })

    const [stateMsg, setStateMsg] = useState(null);

    const [stateItens, setStateItens] = useState([]);

    const excluir = (item) => {
        if (!confirm(`Excluir: ${item.descricao}`)){
            return;
        }

        let itens = [... stateItens];
        let novosItens = itens.filter(e => e.id != item.id)
        setStateItens(novosItens)
    }

    const editar = (itemEditar) => {
        setStateItem({
            id: itemEditar.id,
            descricao: itemEditar.descricao
        })
    }

    const adicionar = () => {
        if (isEmpty(stateItem.descricao))
            setStateMsg({
                tipo: "danger",
                texto: "Tarefa não informada."
            })
        else
        {
            let itens = [... stateItens]
            let item = {
                id: "",
                descricao: stateItem.descricao
            }

            if (stateItem.id == "")
            {
                item.id = gerarId();
                itens.push(item)
            }
            else
            {
                let item = itens.find(f => f.id = stateItem.id)
                item.descricao = stateItem.descricao
            }

            setStateItens(itens)

            setStateItem({
                id: "",
                descricao: ""
            })

            setStateMsg({
                tipo: "success",
                texto: "Tarefa adicionada."
            })
        }
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
                        let editando = item.id == stateItem

                        return (
                            <tr 
                                key={"tarefa-"+index}
                                style={!editando ? {opacity: 0.5} : null} >

                                <td>{item.descricao}</td>
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
        </div>

        return saida;
    }

    return (
        <div>
            <h1>Todo List</h1>
            {renderForm()}
            {renderItens()}
        </div>
    );
}