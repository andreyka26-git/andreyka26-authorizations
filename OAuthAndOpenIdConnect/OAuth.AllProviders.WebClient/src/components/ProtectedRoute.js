import { Navigate } from "react-router-dom";

function ProtectedRoute({authenticated, children, redirectPath = '/'}) {
    if (!authenticated) {
        return <Navigate to={redirectPath} replace />;
    }

    return children;
};

export default ProtectedRoute;