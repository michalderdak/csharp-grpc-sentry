using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using SharpRaven;
using SharpRaven.Data;

namespace NetGrpcSentry
{
    public class SentryInterceptor : Interceptor
    {
        private readonly RavenClient _sentryClient;
        private readonly Action<Exception> _onExceptionCapture;
        private readonly IEnumerable<StatusCode> _statusCodes;

        /// <summary>
        /// General constructor for exception logging
        /// </summary>
        /// <param name="dsn">Sentry dsn</param>
        /// <param name="exceptionStatusCodes">When one of these status codes will be captured, exception won't be logged</param>
        /// <param name="jsonPacketFactory">Sentry configuration</param>
        /// <param name="sentryRequestFactory">Sentry configuration</param>
        /// <param name="sentryUserFactory">Sentry configuration</param>
        public SentryInterceptor(string dsn, IEnumerable<StatusCode> exceptionStatusCodes = null, IJsonPacketFactory jsonPacketFactory = null,
            ISentryRequestFactory sentryRequestFactory = null, ISentryUserFactory sentryUserFactory = null)
        {
            _sentryClient = new RavenClient(dsn, jsonPacketFactory, sentryRequestFactory, sentryUserFactory);
            _onExceptionCapture = null;
            _statusCodes = exceptionStatusCodes;
        }

        /// <summary>
        /// General constructor for exception logging
        /// </summary>
        /// <param name="dsn">Sentry dsn</param>
        /// <param name="exceptionStatusCodes">When one of these status codes will be captured, exception won't be logged</param>
        /// <param name="jsonPacketFactory">Sentry configuration</param>
        /// <param name="sentryRequestFactory">Sentry configuration</param>
        /// <param name="sentryUserFactory">Sentry configuration</param>
        public SentryInterceptor(Dsn dsn, IEnumerable<StatusCode> exceptionStatusCodes = null, IJsonPacketFactory jsonPacketFactory = null,
            ISentryRequestFactory sentryRequestFactory = null, ISentryUserFactory sentryUserFactory = null)
        {
            _sentryClient = new RavenClient(dsn, jsonPacketFactory, sentryRequestFactory, sentryUserFactory);
            _onExceptionCapture = null;
            _statusCodes = exceptionStatusCodes;
        }

        /// <summary>
        /// General constructor for exception logging
        /// </summary>
        /// <param name="exceptionStatusCodes">When one of these status codes will be captured, exception won't be logged</param>
        /// <param name="jsonPacketFactory">Sentry configuration</param>
        public SentryInterceptor(IEnumerable<StatusCode> exceptionStatusCodes = null, IJsonPacketFactory jsonPacketFactory = null)
        {
            _sentryClient = new RavenClient(jsonPacketFactory);
            _onExceptionCapture = null;
            _statusCodes = exceptionStatusCodes;
        }

        /// <summary>
        /// Custom constructor for own action when exception occurs
        /// </summary>
        /// <param name="onExceptionCapture">Action invoked on exception that is suitable for logging</param>
        /// <param name="exceptionStatusCodes">When one of these status codes will be captured, exception won't be logged</param>
        public SentryInterceptor(Action<Exception> onExceptionCapture, IEnumerable<StatusCode> exceptionStatusCodes = null)
        {
            _sentryClient = null;
            _onExceptionCapture = onExceptionCapture;
            _statusCodes = exceptionStatusCodes;
        }

        #region CLIENT

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(request, context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(request, context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(request, context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        #endregion

        #region SERVER

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(request, context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(request, responseStream, context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream, ServerCallContext context,
            ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(requestStream, context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            IServerStreamWriter<TResponse> responseStream, ServerCallContext context,
            DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return continuation(requestStream, responseStream, context);
            }
            catch (RpcException e)
            {
                if (e.StatusCode != StatusCode.Unknown || (_statusCodes != null && _statusCodes.All(c => c != e.StatusCode)))
                {
                    throw;
                }

                if (_onExceptionCapture != null)
                {
                    _onExceptionCapture.Invoke(e);
                }
                else
                {
                    _sentryClient.Capture(new SentryEvent(e));
                }

                throw;
            }
        }

        #endregion
    }
}
