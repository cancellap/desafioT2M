import React from "react";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import styles from "./CreateProjeto.module.css"; // Importando o CSS module
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function CreateProjeto() {
  const validationSchema = Yup.object({
    nome: Yup.string().required("Nome do projeto é obrigatório"),
    descricao: Yup.string().required("Descrição do projeto é obrigatória"),
    dataInicio: Yup.date().required("Data de início é obrigatória").nullable(),
    dataTermino: Yup.date()
      .required("Data de término é obrigatória")
      .nullable(),
    tarefas: Yup.array()
      .of(
        Yup.object({
          nome: Yup.string().required("Nome da tarefa é obrigatório"),
          descricao: Yup.string().required("Descrição da tarefa é obrigatória"),
          prazo: Yup.date().required("Prazo é obrigatório").nullable(),
          statusTarefa: Yup.number()
            .oneOf([0, 1], "Status inválido")
            .required("Status da tarefa é obrigatório"),
        })
      )
      .min(0),
  });

  const initialValues = {
    nome: "",
    descricao: "",
    dataInicio: "",
    dataTermino: "",
    tarefas: [],
  };

  const postProjeto = (projeto) => {
    const url = "http://localhost:5029/api/projeto";
    const token = localStorage.getItem("token");

    if (!token) {
      console.error("Token não encontrado. Faça login novamente.");
      return;
    }
    axios
      .post(url, projeto, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((response) => {
        console.log("Projeto criado com sucesso:", response.data);
      })
      .catch((error) => {
        console.error("Erro ao criar projeto:", error.message);
        if (error.response) {
          console.error("Detalhes do erro:", error.response.data);
        }
        alert("Erro ao criar projeto. Verifique os dados e tente novamente.");
      });
  };

  const navigate = useNavigate();

  const handleSubmit = (values) => {
    postProjeto(values);
    navigate("/projetosGerais")
    console.log(values);
  };

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      {({ errors, touched }) => (
        <div className={styles.formContainer}>
          <h2>Criar Projeto</h2>
          <Form>
            <div className="inputGroup">
              <label htmlFor="nome" className={styles.label}>
                Nome do Projeto
              </label>
              <Field
                name="nome"
                type="text"
                className={`${styles.input} ${
                  touched.nome && errors.nome ? styles.inputError : ""
                }`}
              />
              <ErrorMessage
                name="nome"
                component="div"
                className={styles.errorMessage}
              />
            </div>

            <div className="inputGroup">
              <label htmlFor="descricao" className={styles.label}>
                Descrição
              </label>
              <Field
                name="descricao"
                type="text"
                className={`${styles.input} ${
                  touched.descricao && errors.descricao ? styles.inputError : ""
                }`}
              />
              <ErrorMessage
                name="descricao"
                component="div"
                className={styles.errorMessage}
              />
            </div>

            <div className="inputGroup">
              <label htmlFor="dataInicio" className={styles.label}>
                Data de Início
              </label>
              <Field
                name="dataInicio"
                type="date"
                className={`${styles.input} ${
                  touched.dataInicio && errors.dataInicio
                    ? styles.inputError
                    : ""
                }`}
              />
              <ErrorMessage
                name="dataInicio"
                component="div"
                className={styles.errorMessage}
              />
            </div>

            <div className="inputGroup">
              <label htmlFor="dataTermino" className={styles.label}>
                Data de Término
              </label>
              <Field
                name="dataTermino"
                type="date"
                className={`${styles.input} ${
                  touched.dataTermino && errors.dataTermino
                    ? styles.inputError
                    : ""
                }`}
              />
              <ErrorMessage
                name="dataTermino"
                component="div"
                className={styles.errorMessage}
              />
            </div>

            <button type="submit" className={styles.button}>
              Salvar Projeto
            </button>
          </Form>
        </div>
      )}
    </Formik>
  );
}
