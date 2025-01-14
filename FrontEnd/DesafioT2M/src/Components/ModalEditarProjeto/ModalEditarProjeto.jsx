import React, { useState, useEffect } from "react";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import { updateProjeto } from "../../Service/Api";
import styles from "./ModalEditarProjeto.module.css";

const validationSchema = Yup.object({
  nome: Yup.string().required("Nome do projeto é obrigatório"),
  descricao: Yup.string().required("Descrição é obrigatória"),
  dataInicio: Yup.string().required("Data de início é obrigatória"),
  dataTermino: Yup.string().required("Data de término é obrigatória"),
});

export default function ModalEditarProjeto({ projeto, onClose, onSave }) {
  const [initialValues, setInitialValues] = useState({
    nome: "",
    descricao: "",
    dataInicio: "",
    dataTermino: "",
  });

  useEffect(() => {
    if (projeto) {
      setInitialValues({
        nome: projeto.nome || "",
        descricao: projeto.descricao || "",
        dataInicio: projeto.dataInicio || "",
        dataTermino: projeto.dataTermino || "",
      });
    }
  }, [projeto]);

  const handleSave = async (values) => {
    if (!projeto?.id) {
      console.error("ID do projeto é necessário");
      return;
    }

    const updatedProjeto = {
      nome: values.nome,
      descricao: values.descricao,
      dataInicio: values.dataInicio,
      dataTermino: values.dataTermino,
    };

    try {
      const token = localStorage.getItem("token");
      const updatedData = await updateProjeto(
        projeto.id,
        updatedProjeto,
        token
      );
      console.log("Projeto atualizado com sucesso:", updatedData);
      onSave(updatedData);
      onClose();
    } catch (error) {
      alert(
        error.response?.status !== 200
          ? "Só é possível editar projetos próprios."
          : "Erro ao atualizar o projeto."
      );
    }
    fetchProjetoData()
  };

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <h2>Editar Projeto</h2>
        <Formik
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSave}
        >
          <Form>
            <div className={styles.label}>
              <label>Nome do Projeto:</label>
              <Field className={styles.input} type="text" name="nome" />
              <ErrorMessage
                name="nome"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Descrição:</label>
              <Field className={styles.input} type="text" name="descricao" />
              <ErrorMessage
                name="descricao"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Data de Início:</label>
              <Field className={styles.input} type="date" name="dataInicio" />
              <ErrorMessage
                name="dataInicio"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.label}>
              <label>Data de Término:</label>
              <Field className={styles.input} type="date" name="dataTermino" />
              <ErrorMessage
                name="dataTermino"
                component="div"
                className={styles.error}
              />
            </div>
            <div className={styles.buttons}>
              <button
                className={styles.cancelButton}
                type="button"
                onClick={onClose}
              >
                Cancelar
              </button>
              <button className={styles.button} type="submit">
                Salvar
              </button>
            </div>
          </Form>
        </Formik>
      </div>
    </div>
  );
}
