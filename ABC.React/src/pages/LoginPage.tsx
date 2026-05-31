import React, { useState } from "react"
import { useNavigate } from "react-router-dom"
import { useMutation } from "@apollo/client/react"
import { useTranslation } from "react-i18next"
import {
  LOGIN_MUTATION,
  EXTERNAL_LOGIN_MUTATION,
  DEV_EXTERNAL_LOGIN_MUTATION,
} from "../graphql/operations/authOperations"
import { useAuth } from "../context/AuthContext"
import { useMsal } from "@azure/msal-react"
import { loginRequest } from "../config/msalConfig"
import { GoogleLogin, CredentialResponse } from "@react-oauth/google"

const isDevMode = !!process.env.DEV_MODE

export const LoginPage: React.FC = () => {
  const { t } = useTranslation()
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const [devEmail, setDevEmail] = useState("")
  const [devProvider, setDevProvider] = useState<"GOOGLE" | "AZURE_ENTRA">("GOOGLE")
  const [error, setError] = useState<string | null>(null)
  const [showAdminLogin, setShowAdminLogin] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()
  const { instance: msalInstance } = useMsal()

  const [loginMutation, { loading }] = useMutation(LOGIN_MUTATION, {
    onCompleted: (data) => {
      const { token, email: userEmail, roles } = data.login
      login(token, userEmail, roles)
      navigate("/")
    },
    onError: (err) => setError(err.message),
  })

  const [externalLoginMutation, { loading: externalLoading }] = useMutation(
    EXTERNAL_LOGIN_MUTATION,
    {
      onCompleted: (data) => {
        const { token, email: userEmail, roles } = data.externalLogin
        login(token, userEmail, roles)
        navigate("/")
      },
      onError: (err) => setError(err.message),
    }
  )

  const [devExternalLoginMutation, { loading: devLoading }] = useMutation(
    DEV_EXTERNAL_LOGIN_MUTATION,
    {
      onCompleted: (data) => {
        const { token, email: userEmail, roles } = data.devExternalLogin
        login(token, userEmail, roles)
        navigate("/")
      },
      onError: (err) => setError(err.message),
    }
  )

  const handleAdminSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    loginMutation({ variables: { email, password } })
  }

  const handleMicrosoftLogin = async () => {
    setError(null)
    try {
      const result = await msalInstance.loginPopup(loginRequest)
      externalLoginMutation({
        variables: { provider: "AZURE_ENTRA", idToken: result.idToken },
      })
    } catch (err: unknown) {
      if (err instanceof Error && err.message?.includes("user_cancelled")) return
      setError(
        err instanceof Error ? err.message : t("auth.microsoftLoginFailed")
      )
    }
  }

  const handleGoogleSuccess = (credentialResponse: CredentialResponse) => {
    setError(null)
    if (!credentialResponse.credential) {
      setError(t("auth.googleNoCredential"))
      return
    }
    externalLoginMutation({
      variables: { provider: "GOOGLE", idToken: credentialResponse.credential },
    })
  }

  const handleDevLogin = (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)
    devExternalLoginMutation({
      variables: { provider: devProvider, email: devEmail },
    })
  }

  const isLoading = loading || externalLoading || devLoading

  return (
    <div className="min-h-screen bg-slate-900 flex items-center justify-center px-4">
      <div className="bg-slate-800 rounded-xl shadow-lg p-8 border border-slate-700 w-full max-w-md">
        <h1 className="text-2xl font-bold text-cyan-400 mb-6 text-center">
          {t("app.title")}
        </h1>
        <h2 className="text-lg text-slate-300 mb-6 text-center">
          {t("auth.signIn")}
        </h2>

        {error && (
          <div className="bg-red-900/50 border border-red-700 rounded-lg p-3 mb-4">
            <p className="text-red-300 text-sm">{error}</p>
          </div>
        )}

        {isDevMode && (
          <>
            <div className="bg-purple-900/30 border border-purple-700 rounded-lg p-4 mb-6">
              <p className="text-purple-300 text-xs font-medium mb-3">
                {t("auth.devModeTitle")}
              </p>
              <form onSubmit={handleDevLogin} className="flex flex-col gap-3">
                <input
                  type="email"
                  value={devEmail}
                  onChange={(e) => setDevEmail(e.target.value)}
                  placeholder="user@example.com"
                  required
                  className="w-full bg-slate-700 border border-purple-600 rounded-lg px-3 py-2 text-slate-100 text-sm focus:outline-none focus:border-purple-400"
                />
                <div className="flex gap-2">
                  <button
                    type="button"
                    onClick={() => setDevProvider("GOOGLE")}
                    className={`flex-1 text-xs py-2 rounded-lg transition border ${
                      devProvider === "GOOGLE"
                        ? "bg-purple-700 border-purple-500 text-white"
                        : "bg-slate-700 border-slate-600 text-slate-400"
                    }`}
                  >
                    Google
                  </button>
                  <button
                    type="button"
                    onClick={() => setDevProvider("AZURE_ENTRA")}
                    className={`flex-1 text-xs py-2 rounded-lg transition border ${
                      devProvider === "AZURE_ENTRA"
                        ? "bg-purple-700 border-purple-500 text-white"
                        : "bg-slate-700 border-slate-600 text-slate-400"
                    }`}
                  >
                    Azure Entra
                  </button>
                </div>
                <button
                  type="submit"
                  disabled={isLoading}
                  className="bg-purple-600 hover:bg-purple-500 text-white font-medium py-2 rounded-lg transition disabled:opacity-50 text-sm"
                >
                  {devLoading ? t("auth.signingIn") : t("auth.devLogin")}
                </button>
              </form>
            </div>

            <div className="relative mb-6">
              <div className="absolute inset-0 flex items-center">
                <div className="w-full border-t border-slate-600"></div>
              </div>
              <div className="relative flex justify-center text-sm">
                <span className="bg-slate-800 px-3 text-slate-500">
                  {t("auth.orUseRealProviders")}
                </span>
              </div>
            </div>
          </>
        )}

        <div className="flex flex-col gap-3 mb-6">
          <button
            onClick={handleMicrosoftLogin}
            disabled={isLoading}
            className="flex items-center justify-center gap-3 w-full bg-[#2f2f2f] hover:bg-[#3b3b3b] text-white font-medium py-3 rounded-lg transition disabled:opacity-50 border border-slate-600"
          >
            <svg className="w-5 h-5" viewBox="0 0 21 21" xmlns="http://www.w3.org/2000/svg">
              <rect x="1" y="1" width="9" height="9" fill="#f25022" />
              <rect x="11" y="1" width="9" height="9" fill="#7fba00" />
              <rect x="1" y="11" width="9" height="9" fill="#00a4ef" />
              <rect x="11" y="11" width="9" height="9" fill="#ffb900" />
            </svg>
            {t("auth.azureLogin")}
          </button>

          <div className="flex justify-center">
            <GoogleLogin
              onSuccess={handleGoogleSuccess}
              onError={() => setError(t("auth.googleLoginFailed"))}
              theme="filled_black"
              size="large"
              width="400"
              text="signin_with"
            />
          </div>
        </div>

        <div className="relative mb-6">
          <div className="absolute inset-0 flex items-center">
            <div className="w-full border-t border-slate-600"></div>
          </div>
          <div className="relative flex justify-center text-sm">
            <button
              onClick={() => setShowAdminLogin(!showAdminLogin)}
              className="bg-slate-800 px-3 text-slate-400 hover:text-slate-300 transition"
            >
              {showAdminLogin
                ? t("auth.hideAdminLogin")
                : t("auth.adminLogin")}
            </button>
          </div>
        </div>

        {showAdminLogin && (
          <form onSubmit={handleAdminSubmit} className="flex flex-col gap-4">
            <div>
              <label className="block text-sm text-slate-400 mb-1">
                {t("auth.email")}
              </label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                className="w-full bg-slate-700 border border-slate-600 rounded-lg px-3 py-2 text-slate-100 focus:outline-none focus:border-cyan-400"
              />
            </div>
            <div>
              <label className="block text-sm text-slate-400 mb-1">
                {t("auth.password")}
              </label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                className="w-full bg-slate-700 border border-slate-600 rounded-lg px-3 py-2 text-slate-100 focus:outline-none focus:border-cyan-400"
              />
            </div>
            <button
              type="submit"
              disabled={isLoading}
              className="bg-cyan-600 hover:bg-cyan-500 text-white font-medium py-2 rounded-lg transition disabled:opacity-50"
            >
              {loading ? t("auth.signingIn") : t("auth.signIn")}
            </button>
          </form>
        )}
      </div>
    </div>
  )
}
