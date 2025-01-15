import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import styles from "./DetalhesProjeto.module.css";
import { FaEdit, FaPlus, FaCheck, FaTrash } from "react-icons/fa";
import ModalEditarTarefa from "../../Components/ModalEditarTarefa/ModalEditarTarefa";
import ModalAddTarefa from "../../Components/ModalAddTarefa/ModalAddTarefa";
import ModalEditarProjeto from "../../Components/ModalEditarProjeto/ModalEditarProjeto";
import {
  fetchProjeto,
  createTarefa,
  deleteTarefa,
  editProjeto,
  deleteProjeto,
} from "../../Service/Api";

export default function DetalhesProjeto() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [projeto, setProjeto] = useState(null);
  const [tarefasComUsuarios, setTarefasComUsuarios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedTarefa, setSelectedTarefa] = useState(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [usuarioNome, setUsuarioNome] = useState("");
  const [isEditProjetoModalOpen, setIsEditProjetoModalOpen] = useState(false);

  const token = localStorage.getItem("token");

  const loadProjetoData = async () => {
    try {
      const data = await fetchProjeto(id);
      setProjeto(data.projeto);
      setUsuarioNome(data.usuarioNome);
      setTarefasComUsuarios(data.tarefasComUsuarios);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadProjetoData();
  }, [id]);

  const getStatus = (statusTarefa) => {
    const status = {
      NaoIniciado: "Não Iniciada",
      EmAndamento: "Em Andamento",
    };
    return status[statusTarefa] || "Desconhecido";
  };

  const handleEditClick = (tarefa) => {
    setSelectedTarefa(tarefa);
    setIsEditModalOpen(true);
  };

  const handleSaveTarefa = async () => {
    await loadProjetoData();
  };

  const handleDeleteTarefa = async (idTarefa) => {
    try {
      await deleteTarefa(idTarefa, token);
      await loadProjetoData();
    } catch (error) {
      alert(
        error.response?.status !== 200
          ? "Só é possível concluir tarefas próprias."
          : "Erro ao excluir a tarefa."
      );
    }
  };

  const handleEditProjeto = async (projetoAtualizado) => {
    try {
      const updatedProjeto = await editProjeto(id, projetoAtualizado);
      setProjeto(updatedProjeto);
      setIsEditProjetoModalOpen(false);
    } catch (err) {
      setError(err.message);
    }
  };

  const handleDeleteProjeto = async (id) => {
    const token = localStorage.getItem("token");
    try {
      console.log(id);
      
      await deleteProjeto(id, token);
      navigate("/projetosGerais");
    } catch (error) {}
  };

  if (loading) return <p>Carregando...</p>;

  return (
    <div className={styles.card}>
      <h1>
        {projeto.nome}
        <FaEdit
          color="black"
          style={{
            fontSize: "20px",
            cursor: "pointer",
            marginLeft: "13",
            marginBottom: "3",
          }}
          onClick={() => setIsEditProjetoModalOpen(true)}
        />
        <FaTrash
          color="black"
          style={{
            fontSize: "20px",
            cursor: "pointer",
            marginLeft: "13",
            marginBottom: "3",
          }}
          onClick={() => handleDeleteProjeto(projeto.id)}
        />
      </h1>
      <p>
        <strong>Criado por:</strong> {usuarioNome}
      </p>
      <p>
        <strong>Início:</strong>{" "}
        {new Date(projeto.dataInicio).toLocaleDateString()}
      </p>
      <p>
        <strong>Término</strong>:{" "}
        {new Date(projeto.dataTermino).toLocaleDateString()}
      </p>
      <p className={styles.descricao}>{projeto.descricao}</p>

      <div className={styles.tarefasContainer}>
        <h2 className={styles.tarefasTitle}>
          Tarefas
          <FaPlus
            color="black"
            size={25}
            style={{ cursor: "pointer" }}
            onClick={() => setIsAddModalOpen(true)}
          />
        </h2>
        {tarefasComUsuarios.length ? (
          <ul className={styles.tarefasLista}>
            {tarefasComUsuarios.map((tarefa) => (
              <li
                key={tarefa.id}
                className={`${styles.tarefaItem} ${
                  tarefa.statusTarefa === "Concluida" ? styles.completed : ""
                }`}
              >
                <span className={styles.tarefaNome}>{tarefa.nome}</span>
                <span className={styles.tarefaUsuario}>
                  {tarefa.usuarioNome}
                </span>
                <span
                  className={`${styles.tarefaStatus} ${
                    tarefa.statusTarefa === "NaoIniciado"
                      ? styles.naoIniciado
                      : tarefa.statusTarefa === "EmAndamento"
                      ? styles.emAndamento
                      : ""
                  }`}
                >
                  {getStatus(tarefa.statusTarefa)}
                </span>
                <span>
                  <FaCheck
                    color="black"
                    style={{
                      fontSize: "20px",
                      cursor: "pointer",
                      marginTop: "1",
                    }}
                    onClick={() => handleDeleteTarefa(tarefa.id)}
                  />
                </span>
                <span>
                  <FaEdit
                    color="black"
                    style={{
                      fontSize: "20px",
                      cursor: "pointer",
                      marginTop: "1",
                    }}
                    onClick={() => handleEditClick(tarefa)}
                  />
                </span>
              </li>
            ))}
          </ul>
        ) : (
          <p>Nenhuma tarefa encontrada.</p>
        )}
      </div>

      <button
        className={styles.button}
        onClick={() => navigate("/projetosGerais")}
      >
        Voltar
      </button>

      {isEditModalOpen && (
        <ModalEditarTarefa
          tarefa={selectedTarefa}
          onClose={() => setIsEditModalOpen(false)}
          onSave={handleSaveTarefa}
        />
      )}
      {isAddModalOpen && (
        <ModalAddTarefa
          projetoId={id}
          onClose={() => {
            setIsAddModalOpen(false);
            loadProjetoData(); // Atualiza os dados do projeto ao fechar o modal
          }}
          onSave={() => {
            loadProjetoData(); // Atualiza os dados do projeto após salvar
          }}
        />
      )}

      {isEditProjetoModalOpen && (
        <ModalEditarProjeto
          projeto={projeto}
          projetoId={id}
          onClose={() => setIsEditProjetoModalOpen(false)}
          onSave={handleEditProjeto}
        />
      )}
    </div>
  );
}
