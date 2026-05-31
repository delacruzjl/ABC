import React from "react"
import { useReactiveVar } from "@apollo/client/react"
import { useTranslation } from "react-i18next"
import { apiErrorVar } from "../state/apiError"

export const OfflineBanner: React.FC = () => {
  const { t } = useTranslation()
  const error = useReactiveVar(apiErrorVar)

  if (!error) return null

  return (
    <div className="bg-red-900/90 border-b border-red-700 px-4 py-3 flex items-center justify-between">
      <div className="flex items-center gap-2">
        <span className="text-red-300 text-lg">⚠</span>
        <p className="text-red-100 text-sm font-medium">{error}</p>
      </div>
      <button
        onClick={() => apiErrorVar(null)}
        className="text-red-300 hover:text-red-100 text-sm font-medium ml-4 transition"
      >
        {t("common.dismiss")}
      </button>
    </div>
  )
}
