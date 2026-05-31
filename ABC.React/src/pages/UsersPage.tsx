import React, { useState } from "react"
import { useQuery, useMutation } from "@apollo/client/react"
import { useTranslation } from "react-i18next"
import { GET_USERS } from "../graphql/operations/childOperations"
import {
  DEACTIVATE_USER_MUTATION,
  REACTIVATE_USER_MUTATION,
  DELETE_USER_MUTATION,
} from "../graphql/operations/userOperations"
import type { UserInfo } from "../types/child"
import { useAuth } from "../context/AuthContext"

interface UsersQueryData {
  users: UserInfo[]
}

export const UsersPage: React.FC = () => {
  const { t } = useTranslation()
  const { user: currentUser } = useAuth()
  const { data, loading, error, refetch } = useQuery<UsersQueryData>(GET_USERS)
  const [actionError, setActionError] = useState<string | null>(null)
  const [confirmDelete, setConfirmDelete] = useState<string | null>(null)

  const [deactivateUser] = useMutation(DEACTIVATE_USER_MUTATION, {
    onCompleted: () => {
      refetch()
      setActionError(null)
    },
    onError: (err) => setActionError(err.message),
  })

  const [reactivateUser] = useMutation(REACTIVATE_USER_MUTATION, {
    onCompleted: () => {
      refetch()
      setActionError(null)
    },
    onError: (err) => setActionError(err.message),
  })

  const [deleteUser] = useMutation(DELETE_USER_MUTATION, {
    onCompleted: () => {
      refetch()
      setActionError(null)
      setConfirmDelete(null)
    },
    onError: (err) => {
      setActionError(err.message)
      setConfirmDelete(null)
    },
  })

  if (loading) return <p className="text-slate-400">{t("users.loading")}</p>
  if (error) {
    return (
      <p className="text-red-400">
        {t("common.error")}: {error.message}
      </p>
    )
  }

  const users = data?.users ?? []
  const currentUserEmail = currentUser?.email

  return (
    <div>
      <h1 className="text-2xl font-bold text-cyan-400 mb-6">{t("users.title")}</h1>

      {actionError && (
        <div className="bg-red-900/50 border border-red-700 rounded-lg p-3 mb-4">
          <p className="text-red-300 text-sm">{actionError}</p>
          <button
            onClick={() => setActionError(null)}
            className="text-red-400 hover:text-red-300 text-xs mt-1"
          >
            {t("common.dismiss")}
          </button>
        </div>
      )}

      <div className="overflow-x-auto">
        <table className="w-full text-left">
          <thead>
            <tr className="border-b border-slate-700">
              <th className="py-3 px-4 text-slate-400 text-sm font-medium">
                {t("users.email")}
              </th>
              <th className="py-3 px-4 text-slate-400 text-sm font-medium">
                {t("users.role")}
              </th>
              <th className="py-3 px-4 text-slate-400 text-sm font-medium">
                {t("users.status")}
              </th>
              <th className="py-3 px-4 text-slate-400 text-sm font-medium">
                {t("users.data")}
              </th>
              <th className="py-3 px-4 text-slate-400 text-sm font-medium">
                {t("users.actions")}
              </th>
            </tr>
          </thead>
          <tbody>
            {users.map((u) => {
              const isCurrentUser = u.email === currentUserEmail
              const isAdmin = u.roles.includes("Admin")

              return (
                <tr key={u.id} className="border-b border-slate-700/50 hover:bg-slate-700/30">
                  <td className="py-3 px-4 text-slate-100">
                    {u.email}
                    {isCurrentUser && (
                      <span className="ml-2 text-xs bg-cyan-900 text-cyan-300 px-2 py-0.5 rounded">
                        {t("users.you")}
                      </span>
                    )}
                  </td>
                  <td className="py-3 px-4">
                    {isAdmin ? (
                      <span className="text-xs bg-amber-900 text-amber-300 px-2 py-0.5 rounded">
                        {t("nav.admin")}
                      </span>
                    ) : (
                      <span className="text-xs bg-slate-600 text-slate-300 px-2 py-0.5 rounded">
                        {t("users.userRole")}
                      </span>
                    )}
                  </td>
                  <td className="py-3 px-4">
                    {u.isActive ? (
                      <span className="text-xs bg-emerald-900 text-emerald-300 px-2 py-0.5 rounded">
                        {t("users.active")}
                      </span>
                    ) : (
                      <span className="text-xs bg-red-900 text-red-300 px-2 py-0.5 rounded">
                        {t("users.inactive")}
                      </span>
                    )}
                  </td>
                  <td className="py-3 px-4 text-slate-400 text-sm">
                    {u.hasChildren && (
                      <span className="mr-2" title={t("users.hasChildren")}>
                        👶 {t("nav.children")}
                      </span>
                    )}
                    {u.hasObservations && (
                      <span title={t("users.hasObservations")}>
                        📋 {t("common.observations")}
                      </span>
                    )}
                    {!u.hasChildren && !u.hasObservations && "—"}
                  </td>
                  <td className="py-3 px-4">
                    {isCurrentUser ? (
                      <span className="text-slate-500 text-sm">—</span>
                    ) : (
                      <div className="flex gap-2">
                        {u.isActive ? (
                          <button
                            onClick={() => deactivateUser({ variables: { userId: u.id } })}
                            className="text-xs bg-amber-800 hover:bg-amber-700 text-amber-200 px-3 py-1 rounded transition"
                          >
                            {t("users.deactivate")}
                          </button>
                        ) : (
                          <button
                            onClick={() => reactivateUser({ variables: { userId: u.id } })}
                            className="text-xs bg-emerald-800 hover:bg-emerald-700 text-emerald-200 px-3 py-1 rounded transition"
                          >
                            {t("users.reactivate")}
                          </button>
                        )}

                        {!u.hasChildren && (
                          <>
                            {confirmDelete === u.id ? (
                              <div className="flex gap-1">
                                <button
                                  onClick={() => deleteUser({ variables: { userId: u.id } })}
                                  className="text-xs bg-red-700 hover:bg-red-600 text-white px-3 py-1 rounded transition"
                                >
                                  {t("common.confirm")}
                                </button>
                                <button
                                  onClick={() => setConfirmDelete(null)}
                                  className="text-xs bg-slate-600 hover:bg-slate-500 text-slate-200 px-3 py-1 rounded transition"
                                >
                                  {t("common.cancel")}
                                </button>
                              </div>
                            ) : (
                              <button
                                onClick={() => setConfirmDelete(u.id)}
                                className="text-xs bg-red-900 hover:bg-red-800 text-red-200 px-3 py-1 rounded transition"
                              >
                                {t("users.delete")}
                              </button>
                            )}
                          </>
                        )}
                      </div>
                    )}
                  </td>
                </tr>
              )
            })}
          </tbody>
        </table>
      </div>
    </div>
  )
}
