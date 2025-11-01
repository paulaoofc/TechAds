import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";

// MSW desabilitado - usando backend real
async function enableMocking() {
  // Descomente para habilitar MSW novamente
  // if (import.meta.env.MODE !== "development") {
  //   return;
  // }
  // const { worker } = await import("./mocks/browser");
  // return worker.start({
  //   onUnhandledRequest: "bypass",
  // });
}

enableMocking().then(() => {
  createRoot(document.getElementById("root")!).render(
    <StrictMode>
      <App />
    </StrictMode>
  );
});
