import React from "react"
import { useMutation } from "@apollo/client/react"
import { useTranslation } from "react-i18next"
import { UPDATE_PREFERRED_LANGUAGE } from "../graphql/operations/userOperations"

export const LanguageSwitcher: React.FC = () => {
  const { i18n, t } = useTranslation()
  const [updateLanguage] = useMutation(UPDATE_PREFERRED_LANGUAGE)

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newLang = e.target.value
    i18n.changeLanguage(newLang)
    localStorage.setItem("abc_language", newLang)
    updateLanguage({ variables: { language: newLang } }).catch(() => {})
  }

  return (
    <select
      value={i18n.resolvedLanguage ?? i18n.language}
      onChange={handleChange}
      className="bg-slate-700 text-slate-200 text-sm rounded px-2 py-1 border border-slate-600 focus:outline-none focus:border-cyan-400"
      aria-label={t("language.label")}
    >
      <option value="en">{t("language.en")}</option>
      <option value="es">{t("language.es")}</option>
    </select>
  )
}
