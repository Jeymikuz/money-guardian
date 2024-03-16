/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import { LoginModel, RegisterUserModel } from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Api<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags AuthEndpoints
   * @name V1AuthRegisterCreate
   * @request POST:/api/v1/auth/register
   * @secure
   */
  v1AuthRegisterCreate = (data: RegisterUserModel, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/v1/auth/register`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags AuthEndpoints
   * @name V1AuthLoginCreate
   * @request POST:/api/v1/auth/login
   * @secure
   */
  v1AuthLoginCreate = (data: LoginModel, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/v1/auth/login`,
      method: "POST",
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    });
  /**
   * No description
   *
   * @tags money.guardian.api
   * @name V1TestList
   * @request GET:/api/v1/test
   * @secure
   */
  v1TestList = (params: RequestParams = {}) =>
    this.request<string, any>({
      path: `/api/v1/test`,
      method: "GET",
      secure: true,
      ...params,
    });
}
