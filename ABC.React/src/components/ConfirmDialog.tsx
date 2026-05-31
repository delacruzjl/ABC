import React from "react"
import { useTranslation } from "react-i18next"

interface Props {
  message: string
  onConfirm: () => void
  onCancel: () => void
}

export const ConfirmDialog: React.FC<Props> = ({
  message,
  onConfirm,
  onCancel,
}) => {
  const { t } = useTranslation()

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60">
      <div className="bg-slate-800 border border-slate-600 rounded-xl shadow-2xl p-6 max-w-sm w-full mx-4">
        <p className="text-slate-100 text-sm mb-6">{message}</p>
        <div className="flex justify-end gap-3">
          <button
            onClick={onCancel}
            className="bg-slate-600 hover:bg-slate-500 text-white font-medium px-4 py-2 rounded-lg transition text-sm"
          >
            {t("common.cancel")}
          </button>
          <button
            onClick={onConfirm}
            className="bg-red-600 hover:bg-red-500 text-white font-medium px-4 py-2 rounded-lg transition text-sm"
          >
            {t("common.delete")}
          </button>
        </div>
      </div>
    </div>
  )
}
